namespace multirun
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnRunAll = new System.Windows.Forms.Button();
            this.chbx1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSingleInstance = new System.Windows.Forms.TextBox();
            this.chbxSingleInstance = new System.Windows.Forms.CheckBox();
            this.chbxCloseTree = new System.Windows.Forms.CheckBox();
            this.chbxRestoreOnClose = new System.Windows.Forms.CheckBox();
            this.chbx2 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxCmdline = new System.Windows.Forms.TextBox();
            this.textBoxExec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nud2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.rbtn2 = new System.Windows.Forms.RadioButton();
            this.rbtn1 = new System.Windows.Forms.RadioButton();
            this.tabctrl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbx1 = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbx2 = new System.Windows.Forms.CheckedListBox();
            this.cmbbx1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nud1 = new System.Windows.Forms.NumericUpDown();
            this.menu1 = new System.Windows.Forms.MenuStrip();
            this.profileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelCloseAll = new System.Windows.Forms.Panel();
            this.btnCloseAll = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud2)).BeginInit();
            this.tabctrl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud1)).BeginInit();
            this.menu1.SuspendLayout();
            this.panelCloseAll.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRunAll
            // 
            this.btnRunAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunAll.Location = new System.Drawing.Point(12, 405);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(75, 23);
            this.btnRunAll.TabIndex = 2;
            this.btnRunAll.Text = "Run All";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            this.btnRunAll.Enter += new System.EventHandler(this.btnRunAll_Enter);
            // 
            // chbx1
            // 
            this.chbx1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbx1.AutoSize = true;
            this.chbx1.Enabled = false;
            this.chbx1.Location = new System.Drawing.Point(10, 225);
            this.chbx1.Name = "chbx1";
            this.chbx1.Size = new System.Drawing.Size(66, 17);
            this.chbx1.TabIndex = 1;
            this.chbx1.Text = "Minimize";
            this.toolTip1.SetToolTip(this.chbx1, "Recommended increasing \"Wait some more\" time.");
            this.chbx1.UseVisualStyleBackColor = true;
            this.chbx1.Click += new System.EventHandler(this.chbx1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxSingleInstance);
            this.groupBox1.Controls.Add(this.chbxSingleInstance);
            this.groupBox1.Controls.Add(this.chbxCloseTree);
            this.groupBox1.Controls.Add(this.chbxRestoreOnClose);
            this.groupBox1.Controls.Add(this.chbx2);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nud2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rbtn2);
            this.groupBox1.Controls.Add(this.rbtn1);
            this.groupBox1.Controls.Add(this.tabctrl1);
            this.groupBox1.Controls.Add(this.cmbbx1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nud1);
            this.groupBox1.Controls.Add(this.chbx1);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(511, 370);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drag&&drop shortcuts/exe here, Drag = rearrange, Del = remove, Ctrl+Enter = run s" +
    "elected";
            // 
            // textBoxSingleInstance
            // 
            this.textBoxSingleInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSingleInstance.Enabled = false;
            this.textBoxSingleInstance.Location = new System.Drawing.Point(9, 342);
            this.textBoxSingleInstance.Name = "textBoxSingleInstance";
            this.textBoxSingleInstance.ReadOnly = true;
            this.textBoxSingleInstance.Size = new System.Drawing.Size(308, 20);
            this.textBoxSingleInstance.TabIndex = 13;
            // 
            // chbxSingleInstance
            // 
            this.chbxSingleInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbxSingleInstance.AutoSize = true;
            this.chbxSingleInstance.Enabled = false;
            this.chbxSingleInstance.Location = new System.Drawing.Point(9, 319);
            this.chbxSingleInstance.Name = "chbxSingleInstance";
            this.chbxSingleInstance.Size = new System.Drawing.Size(98, 17);
            this.chbxSingleInstance.TabIndex = 12;
            this.chbxSingleInstance.Text = "Single instance";
            this.toolTip1.SetToolTip(this.chbxSingleInstance, "Don\'t run if already running");
            this.chbxSingleInstance.UseVisualStyleBackColor = true;
            this.chbxSingleInstance.Click += new System.EventHandler(this.chbxSingleInstance_Click);
            // 
            // chbxCloseTree
            // 
            this.chbxCloseTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbxCloseTree.AutoSize = true;
            this.chbxCloseTree.Enabled = false;
            this.chbxCloseTree.Location = new System.Drawing.Point(335, 345);
            this.chbxCloseTree.Name = "chbxCloseTree";
            this.chbxCloseTree.Size = new System.Drawing.Size(113, 17);
            this.chbxCloseTree.TabIndex = 16;
            this.chbxCloseTree.Text = "Close process tree";
            this.toolTip1.SetToolTip(this.chbxCloseTree, "Close also the process descendants tree");
            this.chbxCloseTree.UseVisualStyleBackColor = true;
            this.chbxCloseTree.Click += new System.EventHandler(this.chbxCloseTree_Click);
            // 
            // chbxRestoreOnClose
            // 
            this.chbxRestoreOnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbxRestoreOnClose.AutoSize = true;
            this.chbxRestoreOnClose.Enabled = false;
            this.chbxRestoreOnClose.Location = new System.Drawing.Point(335, 322);
            this.chbxRestoreOnClose.Name = "chbxRestoreOnClose";
            this.chbxRestoreOnClose.Size = new System.Drawing.Size(106, 17);
            this.chbxRestoreOnClose.TabIndex = 15;
            this.chbxRestoreOnClose.Text = "Restore on close";
            this.toolTip1.SetToolTip(this.chbxRestoreOnClose, "If minimized restore window before closing");
            this.chbxRestoreOnClose.UseVisualStyleBackColor = true;
            this.chbxRestoreOnClose.Click += new System.EventHandler(this.chbxRestoreOnClose_Click);
            // 
            // chbx2
            // 
            this.chbx2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbx2.AutoSize = true;
            this.chbx2.Enabled = false;
            this.chbx2.Location = new System.Drawing.Point(233, 296);
            this.chbx2.Name = "chbx2";
            this.chbx2.Size = new System.Drawing.Size(96, 17);
            this.chbx2.TabIndex = 11;
            this.chbx2.Text = "Always on Top";
            this.toolTip1.SetToolTip(this.chbx2, "Make window always on top.\r\nRecommended increasing \"Wait some more\" time.");
            this.chbx2.UseVisualStyleBackColor = true;
            this.chbx2.Click += new System.EventHandler(this.chbx2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxCmdline);
            this.groupBox2.Controls.Add(this.textBoxExec);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(335, 221);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 95);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Close another process";
            // 
            // textBoxCmdline
            // 
            this.textBoxCmdline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCmdline.Enabled = false;
            this.textBoxCmdline.Location = new System.Drawing.Point(9, 69);
            this.textBoxCmdline.Name = "textBoxCmdline";
            this.textBoxCmdline.Size = new System.Drawing.Size(154, 20);
            this.textBoxCmdline.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxCmdline, "Full or partial parameters");
            this.textBoxCmdline.TextChanged += new System.EventHandler(this.textBoxCmdline_TextChanged);
            // 
            // textBoxExec
            // 
            this.textBoxExec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExec.Enabled = false;
            this.textBoxExec.Location = new System.Drawing.Point(9, 32);
            this.textBoxExec.Name = "textBoxExec";
            this.textBoxExec.Size = new System.Drawing.Size(154, 20);
            this.textBoxExec.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBoxExec, "Full or partial executable path");
            this.textBoxExec.TextChanged += new System.EventHandler(this.textBoxExec_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Command line parameters";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Executable";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 296);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Wait some more (ms)";
            // 
            // nud2
            // 
            this.nud2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nud2.Enabled = false;
            this.nud2.Location = new System.Drawing.Point(117, 294);
            this.nud2.Name = "nud2";
            this.nud2.Size = new System.Drawing.Size(75, 20);
            this.nud2.TabIndex = 10;
            this.nud2.ValueChanged += new System.EventHandler(this.nud2_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(271, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "-1 = infinite";
            // 
            // rbtn2
            // 
            this.rbtn2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbtn2.AutoSize = true;
            this.rbtn2.Enabled = false;
            this.rbtn2.Location = new System.Drawing.Point(10, 271);
            this.rbtn2.Name = "rbtn2";
            this.rbtn2.Size = new System.Drawing.Size(101, 17);
            this.rbtn2.TabIndex = 5;
            this.rbtn2.TabStop = true;
            this.rbtn2.Text = "Wait until Exited";
            this.rbtn2.UseVisualStyleBackColor = true;
            this.rbtn2.Click += new System.EventHandler(this.rbtn1_Click);
            // 
            // rbtn1
            // 
            this.rbtn1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbtn1.AutoSize = true;
            this.rbtn1.Enabled = false;
            this.rbtn1.Location = new System.Drawing.Point(10, 248);
            this.rbtn1.Name = "rbtn1";
            this.rbtn1.Size = new System.Drawing.Size(108, 17);
            this.rbtn1.TabIndex = 4;
            this.rbtn1.TabStop = true;
            this.rbtn1.Text = "Wait until Loaded";
            this.rbtn1.UseVisualStyleBackColor = true;
            this.rbtn1.Click += new System.EventHandler(this.rbtn1_Click);
            // 
            // tabctrl1
            // 
            this.tabctrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabctrl1.Controls.Add(this.tabPage1);
            this.tabctrl1.Controls.Add(this.tabPage2);
            this.tabctrl1.Location = new System.Drawing.Point(6, 19);
            this.tabctrl1.Name = "tabctrl1";
            this.tabctrl1.SelectedIndex = 0;
            this.tabctrl1.Size = new System.Drawing.Size(499, 200);
            this.tabctrl1.TabIndex = 0;
            this.tabctrl1.SelectedIndexChanged += new System.EventHandler(this.tabctrl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbx1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(491, 174);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Items to Run";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbx1
            // 
            this.lbx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbx1.FormattingEnabled = true;
            this.lbx1.Location = new System.Drawing.Point(3, 3);
            this.lbx1.Name = "lbx1";
            this.lbx1.Size = new System.Drawing.Size(485, 169);
            this.lbx1.TabIndex = 0;
            this.lbx1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbx1_ItemCheck);
            this.lbx1.SelectedIndexChanged += new System.EventHandler(this.lbx1_SelectedIndexChanged);
            this.lbx1.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbx1_DragDrop);
            this.lbx1.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbx1_DragEnter);
            this.lbx1.DragOver += new System.Windows.Forms.DragEventHandler(this.lbx1_DragOver);
            this.lbx1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbx1_KeyDown);
            this.lbx1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseDown);
            this.lbx1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseMove);
            this.lbx1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbx2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(491, 174);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Run on Close";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbx2
            // 
            this.lbx2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbx2.FormattingEnabled = true;
            this.lbx2.Location = new System.Drawing.Point(3, 3);
            this.lbx2.Name = "lbx2";
            this.lbx2.Size = new System.Drawing.Size(485, 169);
            this.lbx2.TabIndex = 1;
            this.lbx2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbx1_ItemCheck);
            this.lbx2.SelectedIndexChanged += new System.EventHandler(this.lbx1_SelectedIndexChanged);
            this.lbx2.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbx1_DragDrop);
            this.lbx2.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbx1_DragEnter);
            this.lbx2.DragOver += new System.Windows.Forms.DragEventHandler(this.lbx1_DragOver);
            this.lbx2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbx1_KeyDown);
            this.lbx2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseDown);
            this.lbx2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseMove);
            this.lbx2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseUp);
            // 
            // cmbbx1
            // 
            this.cmbbx1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbbx1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbbx1.Enabled = false;
            this.cmbbx1.FormattingEnabled = true;
            this.cmbbx1.Location = new System.Drawing.Point(158, 225);
            this.cmbbx1.Name = "cmbbx1";
            this.cmbbx1.Size = new System.Drawing.Size(106, 21);
            this.cmbbx1.TabIndex = 3;
            this.cmbbx1.SelectedIndexChanged += new System.EventHandler(this.cmbbx1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Priority";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "> Timeout (ms)";
            // 
            // nud1
            // 
            this.nud1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nud1.Enabled = false;
            this.nud1.Location = new System.Drawing.Point(196, 260);
            this.nud1.Name = "nud1";
            this.nud1.Size = new System.Drawing.Size(69, 20);
            this.nud1.TabIndex = 7;
            this.nud1.ValueChanged += new System.EventHandler(this.nud1_ValueChanged);
            // 
            // menu1
            // 
            this.menu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu1.Location = new System.Drawing.Point(0, 0);
            this.menu1.Name = "menu1";
            this.menu1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menu1.Size = new System.Drawing.Size(535, 24);
            this.menu1.TabIndex = 0;
            this.menu1.Text = "menuStrip1";
            // 
            // profileToolStripMenuItem
            // 
            this.profileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.profileToolStripMenuItem.Name = "profileToolStripMenuItem";
            this.profileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.profileToolStripMenuItem.Text = "&Profile";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveasToolStripMenuItem
            // 
            this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
            this.saveasToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveasToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveasToolStripMenuItem.Text = "Save &as...";
            this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Default";
            this.openFileDialog1.Filter = "Profile (*.mlr)|*.mlr";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Profile (*.mlr)|*.mlr";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(121, 405);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Run Selected";
            this.toolTip1.SetToolTip(this.button3, "(Ctrl+Enter)");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "multirun";
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // panelCloseAll
            // 
            this.panelCloseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCloseAll.BackColor = System.Drawing.Color.Transparent;
            this.panelCloseAll.Controls.Add(this.btnCloseAll);
            this.panelCloseAll.Location = new System.Drawing.Point(442, 400);
            this.panelCloseAll.Name = "panelCloseAll";
            this.panelCloseAll.Size = new System.Drawing.Size(93, 36);
            this.panelCloseAll.TabIndex = 4;
            // 
            // btnCloseAll
            // 
            this.btnCloseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseAll.Location = new System.Drawing.Point(9, 7);
            this.btnCloseAll.Name = "btnCloseAll";
            this.btnCloseAll.Size = new System.Drawing.Size(75, 23);
            this.btnCloseAll.TabIndex = 0;
            this.btnCloseAll.Text = "Close All";
            this.btnCloseAll.UseVisualStyleBackColor = true;
            this.btnCloseAll.Click += new System.EventHandler(this.btnCloseAll_Click);
            this.btnCloseAll.Enter += new System.EventHandler(this.btnRunAll_Enter);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 438);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(535, 20);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 15);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 458);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelCloseAll);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRunAll);
            this.Controls.Add(this.menu1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menu1;
            this.Name = "Form1";
            this.Text = "Multirun";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud2)).EndInit();
            this.tabctrl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud1)).EndInit();
            this.menu1.ResumeLayout(false);
            this.menu1.PerformLayout();
            this.panelCloseAll.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRunAll;
		private System.Windows.Forms.CheckBox chbx1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nud1;
		private System.Windows.Forms.CheckedListBox lbx1;
		private System.Windows.Forms.ComboBox cmbbx1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MenuStrip menu1;
		private System.Windows.Forms.ToolStripMenuItem profileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.TabControl tabctrl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckedListBox lbx2;
		private System.Windows.Forms.RadioButton rbtn2;
		private System.Windows.Forms.RadioButton rbtn1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown nud2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxCmdline;
        private System.Windows.Forms.TextBox textBoxExec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelCloseAll;
        private System.Windows.Forms.Button btnCloseAll;
        private System.Windows.Forms.CheckBox chbx2;
        private System.Windows.Forms.CheckBox chbxCloseTree;
        private System.Windows.Forms.CheckBox chbxRestoreOnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox chbxSingleInstance;
        private System.Windows.Forms.TextBox textBoxSingleInstance;
    }
}

