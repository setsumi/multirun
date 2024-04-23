using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Management;
using System.Windows.Threading;

namespace multirun
{
    public partial class Form1 : Form
    {
        //==============================================================
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        //==============================================================
        private const uint WM_MOUSEMOVE = 0x0200;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        //[DllImport("user32.dll")]
        //static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //==============================================================
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //==============================================================
        private const uint WM_CLOSE = 0x0010;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //==============================================================
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        //-----------------
        private enum GWL : int
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }
        private enum WindowStyles : uint
        {
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_OVERLAPPED = 0x0
        }
        [DllImport("user32.dll")]
        static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        //-----------------
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id,
                        (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        //==============================================================
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const ulong HWND_TOPMOST = ulong.MaxValue; // (HWND)(-1)
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern IntPtr SetWindowPos(IntPtr hWnd, ulong hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);


        //==============================================================
        public class ListItem
        {
            public bool Enabled = true;
            public string File = "";
            public bool Minimize = false;
            public Process Proc = null;
            public int Timewait = -1;
            public int Priority = 3;
            public bool Waitstart = true;
            public int WaitMore = 0;
            public string AnotherExe;
            public string AnotherCmdline;
            public bool AlwaysOnTop = false;
            public bool RestoreOnClose = false;
            public bool CloseTree = false;

            public ListItem(string file)
            {
                this.File = file;
            }
            public ListItem(string file, bool minimize, int timewait, bool enabled, int priority,
                bool waitstart, int waitmore, string anexe, string ancmdline, bool alwaysontop,
                bool restoreonclose, bool closetree)
            {
                this.File = file;
                this.Minimize = minimize;
                this.Timewait = timewait;
                this.Enabled = enabled;
                this.Priority = priority;
                this.Waitstart = waitstart;
                this.WaitMore = waitmore;
                this.AnotherExe = anexe;
                this.AnotherCmdline = ancmdline;
                this.AlwaysOnTop = alwaysontop;
                this.RestoreOnClose = restoreonclose;
                this.CloseTree = closetree;
            }

            public override string ToString() { return File; }
        }

        //==============================================================
        private string appfolder;
        private string configfile;
        private string profilefile;
        private bool newTrayRefreshMethod = true;

        private bool mouse_drag = false;
        private bool mouse_down = false;

        //==============================================================
        public Form1()
        {
            InitializeComponent();
        }

        //==============================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnRunAll;
            this.ActiveControl = lbx1;

            lbx1.AllowDrop = true;
            lbx2.AllowDrop = true;
            lbx1.CheckOnClick = false;
            lbx2.CheckOnClick = false;
            nud1.Maximum = decimal.MaxValue;
            nud1.Minimum = -1;
            nud2.Maximum = decimal.MaxValue;
            nud2.Minimum = 0;
            cmbbx1.Items.Add("Realtime: 24");
            cmbbx1.Items.Add("High: 13");
            cmbbx1.Items.Add("Above Normal: 10");
            cmbbx1.Items.Add("Normal: 8");
            cmbbx1.Items.Add("Below Normal: 6");
            cmbbx1.Items.Add("Idle: 4");
            cmbbx1.SelectedIndex = 3;

            appfolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar;
            configfile = appfolder + "multirun.xml";
            profilefile = appfolder + "Default.mlr";
            UpdateTitle();

            openFileDialog1.InitialDirectory = appfolder;
            saveFileDialog1.InitialDirectory = appfolder;

            ConfigLoad();
            ProfileLoad(profilefile);
        }

        //==============================================================
        private void UpdateTitle()
        {
            this.Text = "Multirun - " + Path.GetFileNameWithoutExtension(profilefile);
        }

        //==============================================================
        private void ConfigLoad()
        {
            if (!File.Exists(configfile)) return;

            XElement level1Element = XElement.Load(configfile).Element("config");
            profilefile = level1Element.Attribute("profile").Value;
            try
            {
                this.Width = Int32.Parse(level1Element.Attribute("window-width").Value);
                this.Height = Int32.Parse(level1Element.Attribute("window-height").Value);
                newTrayRefreshMethod = bool.Parse(level1Element.Attribute("new-tray-refresh-method").Value);
            }
            catch { }
        }

        //==============================================================
        private void ProfileLoad(string file)
        {
            if (!File.Exists(file)) return;

            ProfileLoadList(file, lbx1, "item");
            ProfileLoadList(file, lbx2, "itemclose");

            profilefile = file;
            UpdateTitle();
        }
        private void ProfileLoadList(string file, CheckedListBox listbox, string element)
        {
            listbox.ClearSelected();
            foreach (ListItem item in listbox.Items)
                if (item.Proc != null)
                {
                    item.Proc.Close();
                    item.Proc.Dispose();
                }
            listbox.Items.Clear();

            foreach (XElement level1Element in XElement.Load(file).Elements(element))
            {
                string anexe = string.Empty, ancmdline = string.Empty;
                bool aot = false, restoreonclose = false, closetree = false;
                try
                {
                    // attempt to read parameters added later
                    anexe = level1Element.Attribute("executable").Value;
                    ancmdline = level1Element.Attribute("cmdline").Value;
                    aot = bool.Parse(level1Element.Attribute("alwaysontop").Value.ToString());
                    restoreonclose = bool.Parse(level1Element.Attribute("restoreonclose").Value.ToString());
                    closetree = bool.Parse(level1Element.Attribute("closetree").Value.ToString());
                }
                catch { }
                listbox.Items.Add(new ListItem(
                    level1Element.Attribute("file").Value.ToString(),
                    bool.Parse(level1Element.Attribute("minimize").Value.ToString()),
                    int.Parse(level1Element.Attribute("timewait").Value.ToString()),
                    bool.Parse(level1Element.Attribute("enabled").Value.ToString()),
                    int.Parse(level1Element.Attribute("priority").Value.ToString()),
                    bool.Parse(level1Element.Attribute("waitstart").Value.ToString()),
                    int.Parse(level1Element.Attribute("waitmore").Value.ToString()),
                    anexe, ancmdline, aot, restoreonclose, closetree
                    ), bool.Parse(level1Element.Attribute("enabled").Value.ToString()));
            }
        }

        //==============================================================
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigSave();
            ProfileSave(profilefile);
        }

        //==============================================================
        private void ConfigSave()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode rootNode = doc.CreateElement("root");
            doc.AppendChild(rootNode);

            XmlNode configNode = doc.CreateElement("config");

            XmlAttribute optionAttribute = doc.CreateAttribute("profile");
            optionAttribute.Value = profilefile;
            configNode.Attributes.Append(optionAttribute);

            optionAttribute = doc.CreateAttribute("window-width");
            optionAttribute.Value = this.Width.ToString();
            configNode.Attributes.Append(optionAttribute);

            optionAttribute = doc.CreateAttribute("window-height");
            optionAttribute.Value = this.Height.ToString();
            configNode.Attributes.Append(optionAttribute);

            optionAttribute = doc.CreateAttribute("new-tray-refresh-method");
            optionAttribute.Value = newTrayRefreshMethod.ToString();
            configNode.Attributes.Append(optionAttribute);

            rootNode.AppendChild(configNode);

            doc.Save(configfile);
        }

        //==============================================================
        private void ProfileSave(string file)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode itemsNode = doc.CreateElement("items");
            doc.AppendChild(itemsNode);

            foreach (ListItem item in lbx1.Items)
            {
                itemsNode.AppendChild(ProfileSaveNode(item, doc, "item"));
            }
            foreach (ListItem item in lbx2.Items)
            {
                itemsNode.AppendChild(ProfileSaveNode(item, doc, "itemclose"));
            }

            doc.Save(file);
            profilefile = file;
            UpdateTitle();
        }
        private XmlNode ProfileSaveNode(ListItem item, XmlDocument doc, string element)
        {
            XmlNode itemNode = doc.CreateElement(element);

            XmlAttribute itemAttribute = doc.CreateAttribute("enabled");
            itemAttribute.Value = item.Enabled.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("file");
            itemAttribute.Value = item.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("minimize");
            itemAttribute.Value = item.Minimize.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("waitstart");
            itemAttribute.Value = item.Waitstart.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("timewait");
            itemAttribute.Value = item.Timewait.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("priority");
            itemAttribute.Value = item.Priority.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("waitmore");
            itemAttribute.Value = item.WaitMore.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("executable");
            itemAttribute.Value = item.AnotherExe;
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("cmdline");
            itemAttribute.Value = item.AnotherCmdline;
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("alwaysontop");
            itemAttribute.Value = item.AlwaysOnTop.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("restoreonclose");
            itemAttribute.Value = item.RestoreOnClose.ToString();
            itemNode.Attributes.Append(itemAttribute);

            itemAttribute = doc.CreateAttribute("closetree");
            itemAttribute.Value = item.CloseTree.ToString();
            itemNode.Attributes.Append(itemAttribute);

            return itemNode;
        }

        //==============================================================
        private void btnRunAll_Click(object sender, EventArgs e)
        {
            notifyIcon1.Text = "multirun - Running All";
            notifyIcon1.Visible = true;
            panelCloseAll.BackColor = Color.LightPink;
            this.ActiveControl = btnCloseAll;
            this.Hide();

            foreach (ListItem item in lbx1.Items)
                if (item.Enabled)
                    RunIt(item);
        }

        //==============================================================
        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            panelCloseAll.BackColor = Color.Transparent;
            this.Enabled = false;
            Dispatcher.CurrentDispatcher.InvokeAsync(() => CloseAll(), DispatcherPriority.Background);
        }

        private void CloseAll()
        {
            // close running tasks in reverse order
            for (int i = lbx1.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = (ListItem)lbx1.Items[i];
                if (item.Enabled)
                {
                    Process p = GetActiveProcess(item);
                    if (p != null)
                    {
                        if (item.RestoreOnClose)
                        {
                            if (p.MainWindowHandle != IntPtr.Zero)
                            {
                                if (IsIconic(p.MainWindowHandle)) ShowWindow(p.MainWindowHandle, SW_RESTORE);
                            }
                        }

                        if (item.CloseTree)
                        {
                            var descendants = ProcessHelpers.GetDescendantProcesses(p.Id);
                            CloseProcess(p);
                            foreach (var descendant in descendants)
                            {
                                CloseProcess(descendant);
                            }
                        }
                        else
                        {
                            CloseProcess(p);
                        }
                    }
                }
            }

            // run on-close tasks
            foreach (ListItem item in lbx2.Items)
                if (item.Enabled)
                    RunIt(item);

            if (newTrayRefreshMethod)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = appfolder + "TrayCleanup.exe";
                //startInfo.Arguments = args;
                startInfo.Verb = "runas";
                //startInfo.RedirectStandardOutput = true;
                //startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = true;
                startInfo.CreateNoWindow = true;
                Process processTemp = new Process();
                processTemp.StartInfo = startInfo;
                processTemp.EnableRaisingEvents = true;
                processTemp.Start();
                if (!processTemp.WaitForExit(2000)) throw new Exception(startInfo.FileName + " not exited after 2 seconds.");
            }
            else
            {
                RefreshSystray();
            }
            this.Close();
        }

        private void CloseProcess(Process p)
        {
            if (p == null) return;
            p.Refresh();
            if (p.HasExited)
            {
                p.Close();
                p.Dispose();
            }
            else
            {
                IntPtr hmain = IntPtr.Zero;
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    hmain = p.MainWindowHandle;
                    try { p.CloseMainWindow(); }
                    catch { }
                }
                if (hmain == IntPtr.Zero || !p.WaitForExit(1000))
                {
                    foreach (var handle in EnumerateProcessWindowHandles(p.Id))
                    {
                        IntPtr style = GetWindowLong(handle, (int)GWL.GWL_STYLE);
                        if (((UInt64)style & (UInt64)WindowStyles.WS_POPUP) != 0)
                        {
                        }
                        else if (((UInt64)style & (UInt64)WindowStyles.WS_CHILD) != 0)
                        {
                        }
                        else // WS_OVERLAPPED
                        {
                            // close all overlapped windows
                            if (handle != hmain)
                                PostMessage(handle, WM_CLOSE, 0, 0);
                        }
                    }
                    if (!p.WaitForExit(1000))
                    {
                        p.Kill();
                    }
                }
                p.Close();
                p.Dispose();
                Thread.Sleep(50);
            }
        }

        //==============================================================
        private Process GetActiveProcess(ListItem item, string prm_exe = "", string prm_cmdline = "")
        {
            Process ret = null;

            if (string.IsNullOrEmpty(prm_exe))
            {
                prm_exe = item.AnotherExe;
                prm_cmdline = item.AnotherCmdline;
            }

            if (!string.IsNullOrEmpty(prm_exe)) // another process
            {
                // cleanup previous process
                if (item.Proc != null)
                {
                    item.Proc.Close();
                    item.Proc.Dispose();
                    item.Proc = null;
                }

                Process[] processes = Process.GetProcesses();
                foreach (Process proc in processes)
                {
                    string file = null;
                    try { file = proc.MainModule.FileName; }
                    catch { }
                    if (!string.IsNullOrEmpty(file) && -1 != file.IndexOf(prm_exe, StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(prm_cmdline))
                        {
                            item.Proc = proc;
                            ret = proc;
                            break;
                        }
                        else
                        {
                            try
                            {
                                using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + proc.Id))
                                using (var objects = searcher.Get())
                                {
                                    var result = objects.Cast<ManagementBaseObject>().SingleOrDefault();
                                    string cmdline = result?["CommandLine"]?.ToString() ?? string.Empty;
                                    if (cmdline.Contains(prm_cmdline))
                                    {
                                        item.Proc = proc;
                                        ret = proc;
                                        break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            else // same started process
            {
                if (item.Proc != null)
                {
                    item.Proc.Refresh();
                    if (!item.Proc.HasExited)
                    {
                        ret = item.Proc;
                    }
                    else
                    {
                        string exe = GetExecutable(item.ToString());

                        Process[] processes = Process.GetProcesses();
                        foreach (Process proc in processes)
                        {
                            string file = null;
                            try { file = proc.MainModule.FileName; }
                            catch { }
                            if (file != null && string.Equals(file, exe, StringComparison.OrdinalIgnoreCase))
                            {
                                ret = proc;
                                // cleanup previous process
                                item.Proc.Close();
                                item.Proc.Dispose();
                                item.Proc = proc;

                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        //==============================================================
        private string GetExecutable(string link)
        {
            string file = link.ToString();
            string path = "";

            if (file.Substring(file.Length - 4).ToLower() == @".lnk")
            {
                if (File.Exists(file))
                {
                    string name, descr, workdir, args; // not used
                    string err = GetShortcutInfo(file, out name, out path, out descr, out workdir, out args);
                    if (err.Length > 0)
                        MessageBox.Show(err);
                }
            }
            else
            {
                path = file;
            }
            return path;
        }

        //==============================================================
        private void RunIt(ListItem item)
        {
            string file = item.ToString();
            string name, descr; // not used
            string path, workdir, args;

            if (!File.Exists(file)) return;

            if (file.Substring(file.Length - 4).ToLower() == @".lnk")
            {
                string err = GetShortcutInfo(file, out name, out path, out descr, out workdir, out args);
                if (err.Length > 0)
                    MessageBox.Show(err);
            }
            else
            {
                path = file;
                workdir = file.Substring(0, file.LastIndexOf("\\"));
                args = "";
            }

            if (!File.Exists(path)) return;

            //check of already running
            if (item.Proc != null)
            {
                item.Proc.Refresh();
                if (!item.Proc.HasExited)
                {
                    return;
                }
                else
                {
                    item.Proc.Close();
                    item.Proc.Dispose();
                }
            }
            if (string.IsNullOrEmpty(item.AnotherExe))
                item.Proc = GetActiveProcess(item, path, args);
            else
                item.Proc = GetActiveProcess(item);
            if (item.Proc != null)
                return;

            // start new process
            Process proc = new Process();
            item.Proc = proc;
            proc.StartInfo.FileName = path;
            proc.StartInfo.WorkingDirectory = workdir;
            proc.StartInfo.Arguments = args;
            proc.Start();
            try
            {
                if (item.Timewait < 0)
                {
                    if (item.Waitstart)
                        proc.WaitForInputIdle();
                    else
                        proc.WaitForExit();
                }
                else
                {
                    if (item.Waitstart)
                        proc.WaitForInputIdle(item.Timewait);
                    else
                        proc.WaitForExit(item.Timewait);
                }
            }
            catch { }

            if (item.WaitMore > 0)
            {
                Thread.Sleep(item.WaitMore);
            }

            Thread.Sleep(200);

            proc.Refresh();
            if (!proc.HasExited)
            {
                try { proc.PriorityClass = PriorityConvert(item.Priority); }
                catch { }

                if (proc.MainWindowHandle != IntPtr.Zero)
                {
                    if (item.AlwaysOnTop)
                        SetWindowPos(proc.MainWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                    if (item.Minimize)
                        ShowWindow(proc.MainWindowHandle, SW_MINIMIZE);
                }
            }
        }

        //==============================================================
        // Get information about this link.
        // Return an error message if there's a problem.
        private string GetShortcutInfo(string full_name,
                out string name, out string path, out string descr,
                out string working_dir, out string args)
        {
            name = "";
            path = "";
            descr = "";
            working_dir = "";
            args = "";
            try
            {
                // Make a Shell object.
                Shell32.Shell shell = new Shell32.Shell();

                // Get the shortcut's folder and name.
                string shortcut_path =
                        full_name.Substring(0, full_name.LastIndexOf("\\"));
                string shortcut_name =
                        full_name.Substring(full_name.LastIndexOf("\\") + 1);
                if (!shortcut_name.EndsWith(".lnk"))
                    shortcut_name += ".lnk";

                // Get the shortcut's folder.
                Shell32.Folder shortcut_folder =
                        shell.NameSpace(shortcut_path);

                // Get the shortcut's file.
                Shell32.FolderItem folder_item =
                        shortcut_folder.Items().Item(shortcut_name);

                if (folder_item == null)
                    return "Cannot find shortcut file '" + full_name + "'";
                if (!folder_item.IsLink)
                    return "File '" + full_name + "' isn't a shortcut.";

                // Display the shortcut's information.
                Shell32.ShellLinkObject lnk =
                        (Shell32.ShellLinkObject)folder_item.GetLink;
                name = folder_item.Name;
                descr = lnk.Description;
                path = lnk.Path;
                working_dir = lnk.WorkingDirectory;
                args = lnk.Arguments;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //==============================================================
        private ProcessPriorityClass PriorityConvert(int priority)
        {
            switch (priority)
            {
                case 0:
                    return ProcessPriorityClass.RealTime;
                case 1:
                    return ProcessPriorityClass.High;
                case 2:
                    return ProcessPriorityClass.AboveNormal;
                case 3:
                    return ProcessPriorityClass.Normal;
                case 4:
                    return ProcessPriorityClass.BelowNormal;
                case 5:
                    return ProcessPriorityClass.Idle;
                default:
                    return ProcessPriorityClass.Normal;
            }
        }

        //==============================================================
        private void lbx1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_down = true;
        }
        //==============================================================
        private void lbx1_MouseMove(object sender, MouseEventArgs e)
        {
            CheckedListBox listbox = (CheckedListBox)sender;
            if (mouse_down && !mouse_drag)
            {
                mouse_drag = true;

                int ix = listbox.IndexFromPoint(e.Location);
                if (ix != -1)
                {
                    listbox.DoDragDrop(ix.ToString(), DragDropEffects.Move);
                }
            }
        }

        //==============================================================
        private void lbx1_DragDrop(object sender, DragEventArgs e)
        {
            CheckedListBox listbox = (CheckedListBox)sender;
            mouse_down = false;
            mouse_drag = false;

            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                int dix = Convert.ToInt32(e.Data.GetData(DataFormats.Text));//changed this line
                int ix = listbox.IndexFromPoint(listbox.PointToClient(new Point(e.X, e.Y)));
                if (ix != -1)
                {
                    ListItem obj = (ListItem)listbox.Items[dix];
                    listbox.Items.Remove(obj);
                    listbox.Items.Insert(ix, obj);
                    listbox.SetItemChecked(ix, obj.Enabled);
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    listbox.Items.Add(new ListItem(file), true);
                }
            }
        }

        //==============================================================
        private void lbx1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        //==============================================================
        private void lbx1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        //==============================================================
        private void lbx1_KeyDown(object sender, KeyEventArgs e)
        {
            CheckedListBox listbox = (CheckedListBox)sender;
            if (e.KeyCode == Keys.Delete)
            {
                if (listbox.SelectedItem != null)
                    listbox.Items.Remove(listbox.SelectedItem);
            }
            else if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Control)
            {
                button3_Click(null, null);
            }
        }

        //==============================================================
        private void lbx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (CheckedListBox)sender;
            if (listbox.SelectedItem != null)
            {
                chbx1.Enabled = true;
                chbx1.Checked = ((ListItem)listbox.SelectedItem).Minimize;
                nud1.Enabled = true;
                nud1.Value = ((ListItem)listbox.SelectedItem).Timewait;
                cmbbx1.Enabled = true;
                cmbbx1.SelectedIndex = ((ListItem)listbox.SelectedItem).Priority;
                rbtn1.Enabled = true;
                rbtn2.Enabled = true;
                if (((ListItem)listbox.SelectedItem).Waitstart)
                    rbtn1.Checked = true;
                else
                    rbtn2.Checked = true;
                nud2.Enabled = true;
                nud2.Value = ((ListItem)listbox.SelectedItem).WaitMore;
                chbx2.Enabled = true;
                chbx2.Checked = ((ListItem)listbox.SelectedItem).AlwaysOnTop;
                textBoxExec.Enabled = true;
                textBoxExec.Text = ((ListItem)listbox.SelectedItem).AnotherExe;
                textBoxCmdline.Enabled = true;
                textBoxCmdline.Text = ((ListItem)listbox.SelectedItem).AnotherCmdline;
                chbxRestoreOnClose.Enabled = true;
                chbxRestoreOnClose.Checked = ((ListItem)listbox.SelectedItem).RestoreOnClose;
                chbxCloseTree.Enabled = true;
                chbxCloseTree.Checked = ((ListItem)listbox.SelectedItem).CloseTree;

                button3.Enabled = true;
            }
            else
            {
                chbx1.Checked = false;
                chbx1.Enabled = false;
                nud1.Value = 0;
                nud1.Enabled = false;
                cmbbx1.SelectedIndex = 3;
                cmbbx1.Enabled = false;
                rbtn1.Checked = false;
                rbtn2.Checked = false;
                rbtn1.Enabled = false;
                rbtn2.Enabled = false;
                nud2.Value = 0;
                nud2.Enabled = false;
                chbx2.Checked = false;
                chbx2.Enabled = false;
                textBoxExec.Text = string.Empty;
                textBoxExec.Enabled = false;
                textBoxCmdline.Text = string.Empty;
                textBoxCmdline.Enabled = false;
                chbxRestoreOnClose.Checked = false;
                chbxRestoreOnClose.Enabled = false;
                chbxCloseTree.Checked = false;
                chbxCloseTree.Enabled = false;

                button3.Enabled = false;
            }
        }

        //==============================================================
        private void lbx1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
            mouse_drag = false;
        }

        //==============================================================
        private void lbx1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox listbox = (CheckedListBox)sender;
            ((ListItem)listbox.Items[e.Index]).Enabled = (e.NewValue == CheckState.Checked);
        }

        //==============================================================
        private void chbx1_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            ((ListItem)listbox.SelectedItem).Minimize = chbx1.Checked;
        }
        //==============================================================
        private void nud1_ValueChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
                ((ListItem)listbox.SelectedItem).Timewait = (int)nud1.Value;
        }
        //--------------------------------------------------------------
        private void nud2_ValueChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
                ((ListItem)listbox.SelectedItem).WaitMore = (int)nud2.Value;
        }
        //==============================================================
        private void cmbbx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
                ((ListItem)listbox.SelectedItem).Priority = cmbbx1.SelectedIndex;
        }
        //==============================================================
        private void chbx2_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            ((ListItem)listbox.SelectedItem).AlwaysOnTop = chbx2.Checked;
        }
        //==============================================================
        private void textBoxExec_TextChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
                ((ListItem)listbox.SelectedItem).AnotherExe = textBoxExec.Text;
        }
        //--------------------------------------------------------------
        private void textBoxCmdline_TextChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
                ((ListItem)listbox.SelectedItem).AnotherCmdline = textBoxCmdline.Text;
        }
        //==============================================================
        private void tabctrl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbx1_SelectedIndexChanged((lbx1.Visible) ? lbx1 : lbx2, null);
        }
        //==============================================================
        private void rbtn1_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            ((ListItem)listbox.SelectedItem).Waitstart = rbtn1.Checked;
        }
        //==============================================================
        private void chbxRestoreOnClose_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            ((ListItem)listbox.SelectedItem).RestoreOnClose = chbxRestoreOnClose.Checked;
        }
        //==============================================================
        private void chbxCloseTree_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            ((ListItem)listbox.SelectedItem).CloseTree = chbxCloseTree.Checked;
        }

        //==============================================================
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ProfileLoad(openFileDialog1.FileName);
            }
        }

        //==============================================================
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfileSave(profilefile);
        }

        //==============================================================
        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ProfileSave(saveFileDialog1.FileName);
            }
        }

        //==============================================================
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //==============================================================
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            MessageBox.Show("Multirun v" + fvi.FileVersion + "\n\nRight Click minimize button - minimize to tray", "About");
        }

        //==============================================================
        private void button3_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
                RunIt((ListItem)listbox.SelectedItem);
        }

        //==============================================================
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            this.Activate();
        }

        //==============================================================
        private void RefreshSystray()
        {
            IntPtr hNotificationArea = FindWindowEx(FindWindowEx(FindWindowEx(FindWindowEx(
              IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null),
              IntPtr.Zero, "TrayNotifyWnd", null),
              IntPtr.Zero, "SysPager", null),
              IntPtr.Zero, "ToolbarWindow32", null);
            RECT r;
            GetClientRect(hNotificationArea, out r);

            //Now we've got the area, force it to update
            //by sending mouse messages to it.
            int x = 0, y = 0;
            while (x < r.Right)
            {
                while (y < r.Bottom)
                {
                    PostMessage(hNotificationArea, WM_MOUSEMOVE, 0, (y << 16) + x);
                    y += 5;
                }
                y = 0;
                x += 5;
            }
        }

        //==============================================================
        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int HTMINBUTTON = 8;

        protected override void WndProc(ref Message m)
        {
            // no client area
            if (m.Msg == WM_NCRBUTTONDOWN) // right click
            {
                if (m.WParam == (IntPtr)HTMINBUTTON) // minimize button
                {
                    notifyIcon1.Visible = true;
                    this.Hide();
                    return; // do not process
                }
            }
            base.WndProc(ref m);
        }

        //==============================================================
    }
}
