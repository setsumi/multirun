﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace multirun
{
	public partial class Form1 : Form
	{
		//==============================================================
		private const int SW_MAXIMIZE = 3;
		private const int SW_MINIMIZE = 6;

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


		//==============================================================
		private const uint WM_CLOSE = 0x0010;
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		//-----------------
		//private const uint GW_OWNER = 4;
		//[DllImport("user32.dll", SetLastError = true)]
		//private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

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
		public class ListItem
		{
			public bool Enabled = true;
			public string File = "";
			public bool Minimize = false;
			public Process Proc = null;
			public int Timewait = -1;
			public int Priority = 3;

			public ListItem(string file)
			{
				this.File = file;
			}
			public ListItem(string file, bool minimize, int timewait, bool enabled, int priority)
			{
				this.File = file;
				this.Minimize = minimize;
				this.Timewait = timewait;
				this.Enabled = enabled;
				this.Priority = priority;
			}

			public override string ToString() { return File; }
		}

		//==============================================================
		private readonly string configfile = @"multirun.xml";

		private bool mouse_drag = false;
		private bool mouse_down = false;

		//==============================================================
		public Form1()
		{
			InitializeComponent();

			lbx1.AllowDrop = true;
		}

		//==============================================================
		private void Form1_Load(object sender, EventArgs e)
		{
			this.AcceptButton = button1;
			lbx1.CheckOnClick = false;
			nud1.Maximum = decimal.MaxValue;
			nud1.Minimum = -1;
			cmbbx1.Items.Add("Realtime: 24");
			cmbbx1.Items.Add("High: 13");
			cmbbx1.Items.Add("Above Normal: 10");
			cmbbx1.Items.Add("Normal: 8");
			cmbbx1.Items.Add("Below Normal: 6");
			cmbbx1.Items.Add("Idle: 4");
			cmbbx1.SelectedIndex = 3;

			ConfigLoad();
		}

		//==============================================================
		private void ConfigLoad()
		{
			if (!File.Exists(configfile)) return;

			StringBuilder result = new StringBuilder();
			foreach (XElement level1Element in XElement.Load(configfile).Elements("item"))
			{
				lbx1.Items.Add(new ListItem(
					level1Element.Attribute("file").Value.ToString(),
					bool.Parse(level1Element.Attribute("minimize").Value.ToString()),
					int.Parse(level1Element.Attribute("timewait").Value.ToString()),
					bool.Parse(level1Element.Attribute("enabled").Value.ToString()),
					int.Parse(level1Element.Attribute("priority").Value.ToString())
					), bool.Parse(level1Element.Attribute("enabled").Value.ToString()));
			}
		}

		//==============================================================
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			ConfigSave();
		}

		//==============================================================
		private void ConfigSave()
		{
			XmlDocument doc = new XmlDocument();
			XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			doc.AppendChild(docNode);

			XmlNode itemsNode = doc.CreateElement("items");
			doc.AppendChild(itemsNode);

			foreach (ListItem item in lbx1.Items)
			{
				XmlNode itemNode = doc.CreateElement("item");

				XmlAttribute itemAttribute = doc.CreateAttribute("enabled");
				itemAttribute.Value = item.Enabled.ToString();
				itemNode.Attributes.Append(itemAttribute);

				itemAttribute = doc.CreateAttribute("file");
				itemAttribute.Value = item.ToString();
				itemNode.Attributes.Append(itemAttribute);

				itemAttribute = doc.CreateAttribute("minimize");
				itemAttribute.Value = item.Minimize.ToString();
				itemNode.Attributes.Append(itemAttribute);

				itemAttribute = doc.CreateAttribute("timewait");
				itemAttribute.Value = item.Timewait.ToString();
				itemNode.Attributes.Append(itemAttribute);

				itemAttribute = doc.CreateAttribute("priority");
				itemAttribute.Value = item.Priority.ToString();
				itemNode.Attributes.Append(itemAttribute);

				itemsNode.AppendChild(itemNode);
			}

			doc.Save(configfile);
		}

		//==============================================================
		private void button1_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
			this.ActiveControl = button2;

			foreach (ListItem item in lbx1.Items)
				if (item.Enabled)
					RunIt(item);
		}

		//==============================================================
		private void button2_Click(object sender, EventArgs e)
		{
			for (int i = lbx1.Items.Count - 1; i >= 0; i--)
			{
				ListItem item = (ListItem)lbx1.Items[i];
				if (item.Enabled)
				{
					Process p = GetActiveProcess(item);
					if (p != null)
					{
						if (p.MainWindowHandle != IntPtr.Zero)
						{
							try { p.CloseMainWindow(); }
							catch { }
						}
						else
						{
							foreach (var handle in EnumerateProcessWindowHandles(p.Id))
							{
								PostMessage(handle, WM_CLOSE, 0, 0);
								break; // assuming main window is the first one
							}
						}
						Thread.Sleep(100);
					}
				}
			}
			this.Close();
		}

		//==============================================================
		private Process GetActiveProcess(ListItem item)
		{
			Process ret = null;

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
							break;
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

			Process proc = new Process();
			item.Proc = proc;
			proc.StartInfo.FileName = path;
			proc.StartInfo.WorkingDirectory = workdir;
			proc.StartInfo.Arguments = args;
			proc.Start();
			try
			{
				if (item.Timewait < 0)
					proc.WaitForInputIdle();
				else
					proc.WaitForInputIdle(item.Timewait);
			}
			catch { }
			Thread.Sleep(200);

			proc.Refresh();
			if (!proc.HasExited)
			{
				try { proc.PriorityClass = PriorityConvert(item.Priority); }
				catch { }

				if (item.Minimize && proc.MainWindowHandle != IntPtr.Zero)
					ShowWindow(proc.MainWindowHandle, SW_MINIMIZE);
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
			if (mouse_down && !mouse_drag)
			{
				mouse_drag = true;

				int ix = lbx1.IndexFromPoint(e.Location);
				if (ix != -1)
				{
					lbx1.DoDragDrop(ix.ToString(), DragDropEffects.Move);
				}
			}
		}

		//==============================================================
		private void lbx1_DragDrop(object sender, DragEventArgs e)
		{
			mouse_down = false;
			mouse_drag = false;

			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				int dix = Convert.ToInt32(e.Data.GetData(DataFormats.Text));//changed this line
				int ix = lbx1.IndexFromPoint(lbx1.PointToClient(new Point(e.X, e.Y)));
				if (ix != -1)
				{
					ListItem obj = (ListItem)lbx1.Items[dix];
					lbx1.Items.Remove(obj);
					lbx1.Items.Insert(ix, obj);
					lbx1.SetItemChecked(ix, obj.Enabled);
				}
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string file in files)
				{
					lbx1.Items.Add(new ListItem(file), true);
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
			if (e.KeyCode == Keys.Delete)
			{
				if (lbx1.SelectedItem != null)
					lbx1.Items.Remove(lbx1.SelectedItem);
			}
		}

		//==============================================================
		private void lbx1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbx1.SelectedItem != null)
			{
				chbx1.Enabled = true;
				chbx1.Checked = ((ListItem)lbx1.SelectedItem).Minimize;
				nud1.Enabled = true;
				nud1.Value = ((ListItem)lbx1.SelectedItem).Timewait;
				cmbbx1.Enabled = true;
				cmbbx1.SelectedIndex = ((ListItem)lbx1.SelectedItem).Priority;
			}
			else
			{
				chbx1.Checked = false;
				chbx1.Enabled = false;
				nud1.Value = 0;
				nud1.Enabled = false;
				cmbbx1.SelectedIndex = 3;
				cmbbx1.Enabled = false;
			}
		}

		//==============================================================
		private void lbx1_MouseUp(object sender, MouseEventArgs e)
		{
			mouse_down = false;
			mouse_drag = false;
		}

		//==============================================================
		private void chbx1_Click(object sender, EventArgs e)
		{
			((ListItem)lbx1.SelectedItem).Minimize = chbx1.Checked;
		}
		//==============================================================
		private void nud1_ValueChanged(object sender, EventArgs e)
		{
			if (lbx1.SelectedItem != null)
				((ListItem)lbx1.SelectedItem).Timewait = (int)nud1.Value;
		}

		//==============================================================
		private void lbx1_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			((ListItem)lbx1.Items[e.Index]).Enabled = (e.NewValue == CheckState.Checked);
		}

		//==============================================================
		private void cmbbx1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbx1.SelectedItem != null)
				((ListItem)lbx1.SelectedItem).Priority = cmbbx1.SelectedIndex;
		}


	}
}
