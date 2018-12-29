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
			this.button1 = new System.Windows.Forms.Button();
			this.chbx1 = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbx1 = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.nud1 = new System.Windows.Forms.NumericUpDown();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nud1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(12, 209);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Run All";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// chbx1
			// 
			this.chbx1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chbx1.AutoSize = true;
			this.chbx1.Enabled = false;
			this.chbx1.Location = new System.Drawing.Point(9, 168);
			this.chbx1.Name = "chbx1";
			this.chbx1.Size = new System.Drawing.Size(66, 17);
			this.chbx1.TabIndex = 1;
			this.chbx1.Text = "Minimize";
			this.chbx1.UseVisualStyleBackColor = true;
			this.chbx1.Click += new System.EventHandler(this.chbx1_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lbx1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.nud1);
			this.groupBox1.Controls.Add(this.chbx1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(392, 191);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Drag&&drop items to run here, Drag = rearrange, Del = remove";
			// 
			// lbx1
			// 
			this.lbx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lbx1.FormattingEnabled = true;
			this.lbx1.Location = new System.Drawing.Point(9, 19);
			this.lbx1.Name = "lbx1";
			this.lbx1.Size = new System.Drawing.Size(377, 139);
			this.lbx1.TabIndex = 4;
			this.lbx1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseUp);
			this.lbx1.DragOver += new System.Windows.Forms.DragEventHandler(this.lbx1_DragOver);
			this.lbx1.SelectedIndexChanged += new System.EventHandler(this.lbx1_SelectedIndexChanged);
			this.lbx1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbx1_ItemCheck);
			this.lbx1.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbx1_DragDrop);
			this.lbx1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseMove);
			this.lbx1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbx1_MouseDown);
			this.lbx1.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbx1_DragEnter);
			this.lbx1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbx1_KeyDown);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(119, 169);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Timeout (ms)";
			// 
			// nud1
			// 
			this.nud1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nud1.Enabled = false;
			this.nud1.Location = new System.Drawing.Point(192, 168);
			this.nud1.Name = "nud1";
			this.nud1.Size = new System.Drawing.Size(75, 20);
			this.nud1.TabIndex = 2;
			this.nud1.ValueChanged += new System.EventHandler(this.nud1_ValueChanged);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(329, 209);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Close All";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(416, 243);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Multirun";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nud1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chbx1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nud1;
		private System.Windows.Forms.CheckedListBox lbx1;
	}
}

