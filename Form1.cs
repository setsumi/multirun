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
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //==============================================================
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        //==============================================================
        private const uint WM_CLOSE = 0x0010;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

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

        static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId, bool overlapped)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    if (overlapped)
                    {
                        IntPtr style = GetWindowLong(hWnd, (int)GWL.GWL_STYLE);
                        if (((UInt64)style & (UInt64)WindowStyles.WS_POPUP) != 0) { }
                        else if (((UInt64)style & (UInt64)WindowStyles.WS_CHILD) != 0) { }
                        else // WS_OVERLAPPED
                        {
                            handles.Add(hWnd);
                        }
                    }
                    else
                    {
                        handles.Add(hWnd);
                    }
                    return true;
                }, IntPtr.Zero);

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
            public bool SingleInstance = true;

            public ListItem(string file)
            {
                this.File = file;
            }
            public ListItem(string file, bool minimize, int timewait, bool enabled, int priority,
                bool waitstart, int waitmore, string anexe, string ancmdline, bool alwaysontop,
                bool restoreonclose, bool closetree, bool singleinstance)
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
                this.SingleInstance = singleinstance;
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
            lbx1.Tag = -1; // last selected item
            lbx2.Tag = -1;
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
            toolStripStatusLabel1.Text = "Ready.";

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
        /// <summary>
        /// Depending on (panelCloseAll.BackColor == Color.Transparent)
        /// </summary>
        private void UpdateTitle()
        {
            var config = Path.GetFileNameWithoutExtension(profilefile);
            this.Text = $"Multirun - {config}";
            notifyIcon1.Text = config + (panelCloseAll.BackColor == Color.Transparent ? "" : " - [Running All]");
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
                bool aot = false, restoreonclose = false, closetree = false, singleinstance = true;
                try
                {
                    // attempt to read parameters added later
                    anexe = level1Element.Attribute("executable").Value;
                    ancmdline = level1Element.Attribute("cmdline").Value;
                    aot = bool.Parse(level1Element.Attribute("alwaysontop").Value.ToString());
                    restoreonclose = bool.Parse(level1Element.Attribute("restoreonclose").Value.ToString());
                    closetree = bool.Parse(level1Element.Attribute("closetree").Value.ToString());
                    singleinstance = bool.Parse(level1Element.Attribute("singleinstance").Value.ToString());
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
                    anexe, ancmdline, aot, restoreonclose, closetree, singleinstance
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

            itemAttribute = doc.CreateAttribute("singleinstance");
            itemAttribute.Value = item.SingleInstance.ToString();
            itemNode.Attributes.Append(itemAttribute);

            return itemNode;
        }

        //==============================================================
        private void btnRunAll_Click(object sender, EventArgs e)
        {
            panelCloseAll.BackColor = Color.LightPink;
            UpdateTitle();
            notifyIcon1.Visible = true;
            this.ActiveControl = btnCloseAll;
            this.Hide();

            int i = 0, j = 0;
            foreach (ListItem item in lbx1.Items)
            {
                i++;
                if (item.Enabled)
                {
                    j++;
                    SetStatusText($"Run [{i}/{lbx1.Items.Count}] {item.File}");
                    RunIt(item);
                }
            }
            SetStatusText($"Running [{j}/{lbx1.Items.Count}]");
        }

        //==============================================================
        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            panelCloseAll.BackColor = Color.Transparent;
            btnCloseAll.Enabled = false;
            btnRunAll.Enabled = false;
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            this.ActiveControl = listbox;
            this.Refresh();

            CloseAll();
        }

        private void CloseAll()
        {
            // close running tasks in reverse order
            for (int i = lbx1.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = (ListItem)lbx1.Items[i];
                if (item.Enabled)
                {
                    SetStatusText($"Close [{i + 1}/{lbx1.Items.Count}] {item.File}");

                    Process p = GetActiveProcess(item);
                    if (p != null)
                    {
                        if (item.RestoreOnClose)
                        {
                            foreach (var handle in EnumerateProcessWindowHandles(p.Id, true))
                            {
                                if (IsWindowVisible(handle))
                                    ShowWindow(handle, SW_RESTORE);
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
            int j = 0;
            foreach (ListItem item in lbx2.Items)
            {
                j++;
                if (item.Enabled)
                {
                    SetStatusText($"Post close [{j}/{lbx2.Items.Count}] {item.File}");
                    RunIt(item);
                }
            }

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
                    foreach (var handle in EnumerateProcessWindowHandles(p.Id, true))
                    {
                        // close all overlapped windows
                        if (handle != hmain)
                            PostMessage(handle, WM_CLOSE, 0, 0);
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
        /// <summary>
        /// Check if process is already running and assign to item.Proc
        /// </summary>
        /// <returns>Process object or null</returns>
        /// <exception cref="Exception"></exception>
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
                foreach (var p in processes) { if (p != ret) p.Dispose(); }
            }
            else // same started process
            {
                if (item.Proc != null)
                {
                    item.Proc.Refresh();
                    if (!item.Proc.HasExited)
                    {
                        ret = item.Proc;
                        goto exit_GetActiveProcess; // return
                    }
                    else
                    {
                        item.Proc.Close();
                        item.Proc.Dispose();
                        item.Proc = null;
                    }
                }

                if (item.SingleInstance)
                {
                    GetExecutable(item.ToString(), out string exe, out string args);

                    Process[] processes = Process.GetProcesses();
                    foreach (Process proc in processes)
                    {
                        string file = null;
                        try { file = proc.MainModule.FileName; }
                        catch { }
                        if (file != null && string.Equals(file, exe, StringComparison.OrdinalIgnoreCase))
                        {
                            int rc = ProcessCommandLine.Retrieve(proc, out string parm);
                            if (rc != 0) throw new Exception($"ProcessCommandLine.Retrieve() failed with: {rc}");
                            var cmdLineArray = ProcessCommandLine.CommandLineToArgs(parm);
                            // account for quotes around the first argument and then space
                            parm = cmdLineArray.Count > 1 ? parm.Substring(cmdLineArray[0].Length + 3) : "";
                            if (string.Equals(parm, args))
                            {
                                item.Proc = proc;
                                ret = proc;
                                break;
                            }
                        }
                    }
                    foreach (var p in processes) { if (p != ret) p.Dispose(); }
                }
            }
        exit_GetActiveProcess:
            return ret;
        }

        //==============================================================
        private void GetExecutable(string file, out string exe, out string args)
        {
            exe = "";
            args = "";
            if (Path.GetExtension(file).ToLower() == ".lnk")
            {
                if (!File.Exists(file))
                    throw new Exception($"File doesn't exist: {file}");
                string err = GetShortcutInfo(file, out _, out exe, out _, out _, out args);
                if (!string.IsNullOrEmpty(err))
                    throw new Exception(err);
            }
            else
            {
                exe = file;
            }
        }

        //==============================================================
        private void RunIt(ListItem item)
        {
            string file = item.ToString();
            string path, workdir, args;

            if (!File.Exists(file)) return;

            if (Path.GetExtension(file).ToLower() == ".lnk")
            {
                string err = GetShortcutInfo(file, out _, out path, out _, out workdir, out args);
                if (!string.IsNullOrEmpty(err))
                    throw new Exception(err);
            }
            else
            {
                path = file;
                workdir = Path.GetDirectoryName(file);
                args = "";
            }

            if (!File.Exists(path)) return;

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
            else
            {
                proc.Close();
                proc.Dispose();
                item.Proc = null;
            }
        }

        //==============================================================
        /// <summary>
        /// Get information about this link.
        /// </summary>
        /// <returns>Return an error message if there's a problem.</returns>
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
                chbxSingleInstance.Enabled = true;
                chbxSingleInstance.Checked = (listbox.SelectedItem as ListItem).SingleInstance;
                textBoxSingleInstance.Enabled = true;
                UiUpdateSingleInstanceText((ListItem)listbox.SelectedItem);

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
                chbxSingleInstance.Checked = true;
                chbxSingleInstance.Enabled = false;
                textBoxSingleInstance.Text = string.Empty;
                textBoxSingleInstance.BackColor = SystemColors.Control;
                textBoxSingleInstance.Enabled = false;
                toolTip1.SetToolTip(textBoxSingleInstance, string.Empty);

                button3.Enabled = false;
            }
        }

        //==============================================================
        /// <summary>
        /// Returns error or empty string.
        /// </summary>
        private string GetItemSingleInstanceParams(ListItem item, out string path, out string args,
            out string path0, out string args0, out bool isanother)
        {
            path = "";
            args = "";
            path0 = "";
            args0 = "";
            isanother = false;
            string err = "";
            string file = item.ToString();

            if (!File.Exists(file))
            {
                err = $"Error: file doesn't exist: {file}";
                goto exit_GetItemSingleInstanceParams;
            }

            if (Path.GetExtension(file).ToLower() == ".lnk")
            {
                err = GetShortcutInfo(file, out _, out path, out _, out _, out args);
                if (!string.IsNullOrEmpty(err))
                {
                    err = $"Error: {err}";
                    goto exit_GetItemSingleInstanceParams;
                }
            }
            else
            {
                path = file;
                args = "";
            }
            path0 = path;
            args0 = args;

            if (!File.Exists(path))
            {
                err = $"Error: file doesn't exist: {path}";
                goto exit_GetItemSingleInstanceParams;
            }

            if (!string.IsNullOrEmpty(item.AnotherExe))
            {
                isanother = true;
                path = item.AnotherExe;
                args = item.AnotherCmdline;
            }

        exit_GetItemSingleInstanceParams:
            return err;
        }

        //==============================================================
        /// <returns>Combined command line or error string "Error: (description)"</returns>
        private string GetItemSingleInstanceString(ListItem item, out bool iserror)
        {
            string err = GetItemSingleInstanceParams(item, out string path, out string args,
                out string path0, out string args0, out _);
            iserror = !string.IsNullOrEmpty(err);
            return iserror ? $"{err}\n" : $"{path} {args}\n{path0} {args0}";
        }

        //==============================================================
        /// <summary>
        /// Changes TextBox's BackColor to indicate error.
        /// </summary>
        private void UiUpdateSingleInstanceText(ListItem item)
        {
            string text = GetItemSingleInstanceString(item, out bool iserror);
            string[] textSplit = text.Split('\n');
            textBoxSingleInstance.Text = textSplit[0];
            textBoxSingleInstance.BackColor = iserror ? Color.LightPink : SystemColors.Control;
            toolTip1.SetToolTip(textBoxSingleInstance, textSplit[0] == textSplit[1] ? textSplit[0] : text);
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
            {
                ListItem item = (ListItem)listbox.SelectedItem;
                item.AnotherExe = textBoxExec.Text;
                // update single instance string
                UiUpdateSingleInstanceText(item);
            }
        }
        //--------------------------------------------------------------
        private void textBoxCmdline_TextChanged(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            if (listbox.SelectedItem != null)
            {
                ListItem item = (ListItem)listbox.SelectedItem;
                item.AnotherCmdline = textBoxCmdline.Text;
                // update single instance string
                UiUpdateSingleInstanceText(item);
            }
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
        private void chbxSingleInstance_Click(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            ((ListItem)listbox.SelectedItem).SingleInstance = chbxSingleInstance.Checked;
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
            FlashStatusMessage("Profile saved.");
        }

        //==============================================================
        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ProfileSave(saveFileDialog1.FileName);
                FlashStatusMessage("Profile saved.");
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
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            MessageBox.Show("Multirun v" + fvi.FileVersion + "\n\nRight Click minimize button - minimize to tray."
                + "\nEsc - focus list box or bottom button.", "About");
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
        private void btnRunAll_Enter(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            listbox.ClearSelected();
        }

        //==============================================================
        private void SetStatusText(string txt)
        {
            if (toolStripStatusLabel1.ForeColor != SystemColors.ControlText)
                toolStripStatusLabel1.ForeColor = SystemColors.ControlText;
            toolStripStatusLabel1.Text = txt;
            statusStrip1.Refresh();
        }

        //==============================================================
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
                if (this.ActiveControl != listbox)
                {
                    ActivateListbox(listbox);
                }
                else
                {
                    this.ActiveControl = btnCloseAll.Tag as string == "focus me" ? btnCloseAll : btnRunAll;
                }
            }
        }

        //==============================================================
        private void btnRunAll_Leave(object sender, EventArgs e)
        {
            btnCloseAll.Tag = sender == btnCloseAll ? "focus me" : "";
        }

        //==============================================================
        private void ActivateListbox(ListBox listbox)
        {
            int index = (int)listbox.Tag;
            this.ActiveControl = listbox;
            listbox.SelectedIndex = (index == -1 && listbox.Items.Count > 0) ? 0 : index;
        }

        //==============================================================
        private void lbx1_Leave(object sender, EventArgs e)
        {
            CheckedListBox listbox = (lbx1.Visible) ? lbx1 : lbx2;
            listbox.Tag = listbox.SelectedIndex;
        }

        //==============================================================
        private void FlashStatusMessage(string msg)
        {
            toolStripStatusLabel1.Text = msg;
            tmrStatusMsg.Tag = 0;
            tmrStatusMsg.Start();
        }

        //==============================================================
        private void tmrStatusMsg_Tick(object sender, EventArgs e)
        {
            int counter = (int)tmrStatusMsg.Tag + 1;
            tmrStatusMsg.Tag = counter;
            if (counter > 6)
            {
                tmrStatusMsg.Stop();
                return;
            }
            toolStripStatusLabel1.ForeColor = counter % 2 == 0 ? SystemColors.ControlText : SystemColors.Control;
        }
    } // Form1
}
