namespace QuickMinCombine {
    /// <summary>
    /// Application main form class
    /// </summary>
    public partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && ( components != null ) ) {
                components.Dispose();
            }

            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtSingle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSingle = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.cmsListOps = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.smiUp = new System.Windows.Forms.ToolStripMenuItem();
            this.smiDown = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCombine = new System.Windows.Forms.TextBox();
            this.btnCombineTo = new System.Windows.Forms.Button();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.radCombMin = new System.Windows.Forms.RadioButton();
            this.radCombine = new System.Windows.Forms.RadioButton();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.timeCheck = new System.Windows.Forms.Timer(this.components);
            this.btnAddFile = new System.Windows.Forms.Button();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tssTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssMini = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssReduction = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLast = new System.Windows.Forms.ToolStripStatusLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smiStart = new System.Windows.Forms.ToolStripMenuItem();
            this.smiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.smiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsListOps.SuspendLayout();
            this.grpOperation.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.cmsTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSingle
            // 
            this.txtSingle.Location = new System.Drawing.Point(15, 25);
            this.txtSingle.Name = "txtSingle";
            this.txtSingle.Size = new System.Drawing.Size(367, 20);
            this.txtSingle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Minify Single File";
            // 
            // btnSingle
            // 
            this.btnSingle.Location = new System.Drawing.Point(396, 22);
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(96, 23);
            this.btnSingle.TabIndex = 2;
            this.btnSingle.Text = "Select && Minify";
            this.btnSingle.UseVisualStyleBackColor = true;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            this.btnSingle.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnSingle.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minify and Combine (in given order)";
            // 
            // lstFiles
            // 
            this.lstFiles.ContextMenuStrip = this.cmsListOps;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.Location = new System.Drawing.Point(15, 104);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(367, 160);
            this.lstFiles.TabIndex = 4;
            this.lstFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstFiles_MouseDown);
            // 
            // cmsListOps
            // 
            this.cmsListOps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smiRemove,
            this.toolStripMenuItem1,
            this.smiUp,
            this.smiDown});
            this.cmsListOps.Name = "cmsListOps";
            this.cmsListOps.Size = new System.Drawing.Size(165, 98);
            this.cmsListOps.Opening += new System.ComponentModel.CancelEventHandler(this.cmsListOps_Opening);
            // 
            // smiRemove
            // 
            this.smiRemove.Name = "smiRemove";
            this.smiRemove.Size = new System.Drawing.Size(164, 22);
            this.smiRemove.Text = "Remove from list";
            this.smiRemove.Click += new System.EventHandler(this.smiRemove_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // smiUp
            // 
            this.smiUp.Name = "smiUp";
            this.smiUp.Size = new System.Drawing.Size(164, 22);
            this.smiUp.Text = "Move Up";
            this.smiUp.Click += new System.EventHandler(this.smiUp_Click);
            // 
            // smiDown
            // 
            this.smiDown.Name = "smiDown";
            this.smiDown.Size = new System.Drawing.Size(164, 22);
            this.smiDown.Text = "Move Down";
            this.smiDown.Click += new System.EventHandler(this.smiDown_Click);
            // 
            // btnDirectory
            // 
            this.btnDirectory.Location = new System.Drawing.Point(396, 104);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(96, 23);
            this.btnDirectory.TabIndex = 5;
            this.btnDirectory.Text = "Add Directory";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
            this.btnDirectory.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnDirectory.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(396, 241);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(96, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear List";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnClear.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Combine to...";
            // 
            // txtCombine
            // 
            this.txtCombine.Location = new System.Drawing.Point(15, 295);
            this.txtCombine.Name = "txtCombine";
            this.txtCombine.Size = new System.Drawing.Size(367, 20);
            this.txtCombine.TabIndex = 10;
            // 
            // btnCombineTo
            // 
            this.btnCombineTo.Location = new System.Drawing.Point(396, 292);
            this.btnCombineTo.Name = "btnCombineTo";
            this.btnCombineTo.Size = new System.Drawing.Size(96, 23);
            this.btnCombineTo.TabIndex = 11;
            this.btnCombineTo.Text = "Select File";
            this.btnCombineTo.UseVisualStyleBackColor = true;
            this.btnCombineTo.Click += new System.EventHandler(this.btnCombineTo_Click);
            this.btnCombineTo.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnCombineTo.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.radCombMin);
            this.grpOperation.Controls.Add(this.radCombine);
            this.grpOperation.Location = new System.Drawing.Point(292, 342);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(200, 43);
            this.grpOperation.TabIndex = 12;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "Operation";
            // 
            // radCombMin
            // 
            this.radCombMin.AutoSize = true;
            this.radCombMin.Checked = true;
            this.radCombMin.Location = new System.Drawing.Point(89, 19);
            this.radCombMin.Name = "radCombMin";
            this.radCombMin.Size = new System.Drawing.Size(105, 17);
            this.radCombMin.TabIndex = 1;
            this.radCombMin.TabStop = true;
            this.radCombMin.Text = "Combine && Minify";
            this.radCombMin.UseVisualStyleBackColor = true;
            // 
            // radCombine
            // 
            this.radCombine.AutoSize = true;
            this.radCombine.Location = new System.Drawing.Point(6, 19);
            this.radCombine.Name = "radCombine";
            this.radCombine.Size = new System.Drawing.Size(66, 17);
            this.radCombine.TabIndex = 0;
            this.radCombine.Text = "Combine";
            this.radCombine.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(15, 358);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(91, 23);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start Monitoring";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnStart.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(15, 358);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(91, 23);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            this.btnStop.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnStop.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // timeCheck
            // 
            this.timeCheck.Interval = 1000;
            this.timeCheck.Tick += new System.EventHandler(this.timeCheck_Tick);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Location = new System.Drawing.Point(396, 145);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(96, 23);
            this.btnAddFile.TabIndex = 16;
            this.btnAddFile.Text = "Add Single File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            this.btnAddFile.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnAddFile.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssTotal,
            this.toolStripStatusLabel1,
            this.tssMini,
            this.toolStripStatusLabel2,
            this.tssReduction,
            this.toolStripStatusLabel3,
            this.tssLast});
            this.ssStatus.Location = new System.Drawing.Point(0, 406);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(501, 22);
            this.ssStatus.SizingGrip = false;
            this.ssStatus.TabIndex = 17;
            this.ssStatus.Text = "statusStrip1";
            // 
            // tssTotal
            // 
            this.tssTotal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssTotal.Name = "tssTotal";
            this.tssTotal.Size = new System.Drawing.Size(102, 17);
            this.tssTotal.Text = "Combined Size: 0b";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = " | ";
            // 
            // tssMini
            // 
            this.tssMini.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssMini.Name = "tssMini";
            this.tssMini.Size = new System.Drawing.Size(71, 17);
            this.tssMini.Text = "Post Size: 0b";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel2.Text = " | ";
            // 
            // tssReduction
            // 
            this.tssReduction.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssReduction.Name = "tssReduction";
            this.tssReduction.Size = new System.Drawing.Size(68, 17);
            this.tssReduction.Text = "Change: 0%";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel3.Text = " | ";
            // 
            // tssLast
            // 
            this.tssLast.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssLast.Name = "tssLast";
            this.tssLast.Size = new System.Drawing.Size(75, 17);
            this.tssLast.Text = "Last: 00:00:00";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.label4.Location = new System.Drawing.Point(0, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(501, 1);
            this.label4.TabIndex = 18;
            // 
            // niTray
            // 
            this.niTray.ContextMenuStrip = this.cmsTray;
            this.niTray.Icon = ((System.Drawing.Icon)(resources.GetObject("niTray.Icon")));
            this.niTray.Text = "Combinify";
            this.niTray.Visible = true;
            this.niTray.DoubleClick += new System.EventHandler(this.niTray_DoubleClick);
            // 
            // cmsTray
            // 
            this.cmsTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smiStart,
            this.smiStop,
            this.smiClose});
            this.cmsTray.Name = "cmsTray";
            this.cmsTray.Size = new System.Drawing.Size(163, 70);
            // 
            // smiStart
            // 
            this.smiStart.Enabled = false;
            this.smiStart.Name = "smiStart";
            this.smiStart.Size = new System.Drawing.Size(162, 22);
            this.smiStart.Text = "Start Monitoring";
            this.smiStart.Click += new System.EventHandler(this.smiStart_Click);
            // 
            // smiStop
            // 
            this.smiStop.Enabled = false;
            this.smiStop.Name = "smiStop";
            this.smiStop.Size = new System.Drawing.Size(162, 22);
            this.smiStop.Text = "Stop Monitoring";
            this.smiStop.Click += new System.EventHandler(this.smiStop_Click);
            // 
            // smiClose
            // 
            this.smiClose.Name = "smiClose";
            this.smiClose.Size = new System.Drawing.Size(162, 22);
            this.smiClose.Text = "Close Combinify";
            this.smiClose.Click += new System.EventHandler(this.smiClose_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 428);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSingle);
            this.Controls.Add(this.txtCombine);
            this.Controls.Add(this.btnAddFile);
            this.Controls.Add(this.grpOperation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCombineTo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDirectory);
            this.Controls.Add(this.txtSingle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ssStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Combinify";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.cmsListOps.ResumeLayout(false);
            this.grpOperation.ResumeLayout(false);
            this.grpOperation.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.cmsTray.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSingle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSingle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCombine;
        private System.Windows.Forms.Button btnCombineTo;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.RadioButton radCombMin;
        private System.Windows.Forms.RadioButton radCombine;
        private System.Windows.Forms.ContextMenuStrip cmsListOps;
        private System.Windows.Forms.ToolStripMenuItem smiRemove;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timeCheck;
        private System.Windows.Forms.ToolStripMenuItem smiUp;
        private System.Windows.Forms.ToolStripMenuItem smiDown;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel tssTotal;
        private System.Windows.Forms.ToolStripStatusLabel tssMini;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tssLast;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripStatusLabel tssReduction;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.ContextMenuStrip cmsTray;
        private System.Windows.Forms.ToolStripMenuItem smiStart;
        private System.Windows.Forms.ToolStripMenuItem smiStop;
        private System.Windows.Forms.ToolStripMenuItem smiClose;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}

