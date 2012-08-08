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
            this.smiDir = new System.Windows.Forms.ToolStripMenuItem();
            this.smiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.smiUp = new System.Windows.Forms.ToolStripMenuItem();
            this.smiDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.smiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.smiClear = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCombine = new System.Windows.Forms.TextBox();
            this.btnCombineTo = new System.Windows.Forms.Button();
            this.radCombMin = new System.Windows.Forms.RadioButton();
            this.radCombine = new System.Windows.Forms.RadioButton();
            this.timeCheck = new System.Windows.Forms.Timer(this.components);
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
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.smiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNewDir = new System.Windows.Forms.ToolStripMenuItem();
            this.miProjectOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miList = new System.Windows.Forms.ToolStripMenuItem();
            this.miListDir = new System.Windows.Forms.ToolStripMenuItem();
            this.miListFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miListUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miListDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.miListRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.miListClear = new System.Windows.Forms.ToolStripMenuItem();
            this.miMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.miMonitorStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miMonitorStop = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.radMinify = new System.Windows.Forms.RadioButton();
            this.cmsListOps.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.cmsTray.SuspendLayout();
            this.msMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSingle
            // 
            this.txtSingle.Location = new System.Drawing.Point(15, 54);
            this.txtSingle.Name = "txtSingle";
            this.txtSingle.Size = new System.Drawing.Size(474, 20);
            this.txtSingle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Minify Single File";
            // 
            // btnSingle
            // 
            this.btnSingle.Location = new System.Drawing.Point(393, 80);
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
            this.label2.Location = new System.Drawing.Point(12, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Watch List (Processed in given order)";
            // 
            // lstFiles
            // 
            this.lstFiles.ContextMenuStrip = this.cmsListOps;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.Location = new System.Drawing.Point(15, 154);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(474, 160);
            this.lstFiles.TabIndex = 4;
            this.lstFiles.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            this.lstFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstFiles_MouseDown);
            // 
            // cmsListOps
            // 
            this.cmsListOps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smiDir,
            this.smiFile,
            this.toolStripMenuItem9,
            this.smiUp,
            this.smiDown,
            this.toolStripMenuItem1,
            this.smiRemove,
            this.toolStripMenuItem7,
            this.smiClear});
            this.cmsListOps.Name = "cmsListOps";
            this.cmsListOps.Size = new System.Drawing.Size(165, 154);
            // 
            // smiDir
            // 
            this.smiDir.Name = "smiDir";
            this.smiDir.Size = new System.Drawing.Size(164, 22);
            this.smiDir.Text = "Add Directory";
            this.smiDir.Click += new System.EventHandler(this.MenuAddDir_Click);
            // 
            // smiFile
            // 
            this.smiFile.Name = "smiFile";
            this.smiFile.Size = new System.Drawing.Size(164, 22);
            this.smiFile.Text = "Add File";
            this.smiFile.Click += new System.EventHandler(this.MenuAddFile_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(161, 6);
            // 
            // smiUp
            // 
            this.smiUp.Enabled = false;
            this.smiUp.Name = "smiUp";
            this.smiUp.Size = new System.Drawing.Size(164, 22);
            this.smiUp.Text = "Move Up";
            this.smiUp.Click += new System.EventHandler(this.MenuUp_Click);
            // 
            // smiDown
            // 
            this.smiDown.Enabled = false;
            this.smiDown.Name = "smiDown";
            this.smiDown.Size = new System.Drawing.Size(164, 22);
            this.smiDown.Text = "Move Down";
            this.smiDown.Click += new System.EventHandler(this.MenuDown_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // smiRemove
            // 
            this.smiRemove.Enabled = false;
            this.smiRemove.Name = "smiRemove";
            this.smiRemove.Size = new System.Drawing.Size(164, 22);
            this.smiRemove.Text = "Remove from list";
            this.smiRemove.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(161, 6);
            // 
            // smiClear
            // 
            this.smiClear.Enabled = false;
            this.smiClear.Name = "smiClear";
            this.smiClear.Size = new System.Drawing.Size(164, 22);
            this.smiClear.Text = "Clear List";
            this.smiClear.Click += new System.EventHandler(this.MenuClear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 334);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Destination File";
            // 
            // txtCombine
            // 
            this.txtCombine.Location = new System.Drawing.Point(15, 350);
            this.txtCombine.Name = "txtCombine";
            this.txtCombine.Size = new System.Drawing.Size(474, 20);
            this.txtCombine.TabIndex = 10;
            // 
            // btnCombineTo
            // 
            this.btnCombineTo.Location = new System.Drawing.Point(15, 389);
            this.btnCombineTo.Name = "btnCombineTo";
            this.btnCombineTo.Size = new System.Drawing.Size(96, 23);
            this.btnCombineTo.TabIndex = 11;
            this.btnCombineTo.Text = "Select File";
            this.btnCombineTo.UseVisualStyleBackColor = true;
            this.btnCombineTo.Click += new System.EventHandler(this.btnCombineTo_Click);
            this.btnCombineTo.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnCombineTo.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // radCombMin
            // 
            this.radCombMin.AutoSize = true;
            this.radCombMin.Checked = true;
            this.radCombMin.Location = new System.Drawing.Point(280, 395);
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
            this.radCombine.Location = new System.Drawing.Point(208, 395);
            this.radCombine.Name = "radCombine";
            this.radCombine.Size = new System.Drawing.Size(66, 17);
            this.radCombine.TabIndex = 0;
            this.radCombine.Text = "Combine";
            this.radCombine.UseVisualStyleBackColor = true;
            // 
            // timeCheck
            // 
            this.timeCheck.Interval = 1000;
            this.timeCheck.Tick += new System.EventHandler(this.timeCheck_Tick);
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
            this.ssStatus.Location = new System.Drawing.Point(0, 432);
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
            this.label4.Location = new System.Drawing.Point(0, 113);
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
            this.toolStripMenuItem8,
            this.smiExit});
            this.cmsTray.Name = "cmsTray";
            this.cmsTray.Size = new System.Drawing.Size(162, 76);
            // 
            // smiStart
            // 
            this.smiStart.Enabled = false;
            this.smiStart.Name = "smiStart";
            this.smiStart.Size = new System.Drawing.Size(161, 22);
            this.smiStart.Text = "Start Monitoring";
            this.smiStart.Click += new System.EventHandler(this.MenuStart_Click);
            // 
            // smiStop
            // 
            this.smiStop.Enabled = false;
            this.smiStop.Name = "smiStop";
            this.smiStop.Size = new System.Drawing.Size(161, 22);
            this.smiStop.Text = "Stop Monitoring";
            this.smiStop.Click += new System.EventHandler(this.MenuStop_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(158, 6);
            // 
            // smiExit
            // 
            this.smiExit.Name = "smiExit";
            this.smiExit.Size = new System.Drawing.Size(161, 22);
            this.smiExit.Text = "Exit Combinify";
            this.smiExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // msMainMenu
            // 
            this.msMainMenu.BackColor = System.Drawing.SystemColors.Control;
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miList,
            this.miMonitor});
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new System.Drawing.Size(501, 24);
            this.msMainMenu.TabIndex = 19;
            this.msMainMenu.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileNew,
            this.miProjectOpen,
            this.toolStripMenuItem2,
            this.miFileSave,
            this.miFileSaveAs,
            this.toolStripMenuItem6,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // miFileNew
            // 
            this.miFileNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileNewFile,
            this.miFileNewDir});
            this.miFileNew.Name = "miFileNew";
            this.miFileNew.Size = new System.Drawing.Size(196, 22);
            this.miFileNew.Text = "New Project";
            // 
            // miFileNewFile
            // 
            this.miFileNewFile.Enabled = false;
            this.miFileNewFile.Name = "miFileNewFile";
            this.miFileNewFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miFileNewFile.Size = new System.Drawing.Size(228, 22);
            this.miFileNewFile.Text = "From File";
            this.miFileNewFile.Click += new System.EventHandler(this.miProjectNewFile_Click);
            // 
            // miFileNewDir
            // 
            this.miFileNewDir.Enabled = false;
            this.miFileNewDir.Name = "miFileNewDir";
            this.miFileNewDir.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.miFileNewDir.Size = new System.Drawing.Size(228, 22);
            this.miFileNewDir.Text = "From Directory";
            this.miFileNewDir.Click += new System.EventHandler(this.miProjectNewDir_Click);
            // 
            // miProjectOpen
            // 
            this.miProjectOpen.Enabled = false;
            this.miProjectOpen.Name = "miProjectOpen";
            this.miProjectOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miProjectOpen.Size = new System.Drawing.Size(196, 22);
            this.miProjectOpen.Text = "&Open Project";
            this.miProjectOpen.Click += new System.EventHandler(this.miProjectOpen_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(193, 6);
            // 
            // miFileSave
            // 
            this.miFileSave.Enabled = false;
            this.miFileSave.Name = "miFileSave";
            this.miFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSave.Size = new System.Drawing.Size(196, 22);
            this.miFileSave.Text = "&Save Project";
            this.miFileSave.Click += new System.EventHandler(this.miProjectSave_Click);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Enabled = false;
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.miFileSaveAs.Size = new System.Drawing.Size(196, 22);
            this.miFileSaveAs.Text = "Save Project &As";
            this.miFileSaveAs.Click += new System.EventHandler(this.miProjectSaveAs_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(193, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.miFileExit.Size = new System.Drawing.Size(196, 22);
            this.miFileExit.Text = "E&xit";
            this.miFileExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // miList
            // 
            this.miList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miListDir,
            this.miListFile,
            this.toolStripMenuItem3,
            this.miListUp,
            this.miListDown,
            this.toolStripMenuItem4,
            this.miListRemove,
            this.toolStripMenuItem5,
            this.miListClear});
            this.miList.Name = "miList";
            this.miList.Size = new System.Drawing.Size(37, 20);
            this.miList.Text = "&List";
            // 
            // miListDir
            // 
            this.miListDir.Name = "miListDir";
            this.miListDir.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.miListDir.Size = new System.Drawing.Size(203, 22);
            this.miListDir.Text = "Add &Directory";
            this.miListDir.Click += new System.EventHandler(this.MenuAddDir_Click);
            // 
            // miListFile
            // 
            this.miListFile.Name = "miListFile";
            this.miListFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miListFile.Size = new System.Drawing.Size(203, 22);
            this.miListFile.Text = "Add &File";
            this.miListFile.Click += new System.EventHandler(this.MenuAddFile_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(200, 6);
            // 
            // miListUp
            // 
            this.miListUp.Enabled = false;
            this.miListUp.Name = "miListUp";
            this.miListUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.miListUp.Size = new System.Drawing.Size(203, 22);
            this.miListUp.Text = "Move Up";
            this.miListUp.Click += new System.EventHandler(this.MenuUp_Click);
            // 
            // miListDown
            // 
            this.miListDown.Enabled = false;
            this.miListDown.Name = "miListDown";
            this.miListDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.miListDown.Size = new System.Drawing.Size(203, 22);
            this.miListDown.Text = "Move Down";
            this.miListDown.Click += new System.EventHandler(this.MenuDown_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(200, 6);
            // 
            // miListRemove
            // 
            this.miListRemove.Enabled = false;
            this.miListRemove.Name = "miListRemove";
            this.miListRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miListRemove.Size = new System.Drawing.Size(203, 22);
            this.miListRemove.Text = "Remove";
            this.miListRemove.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(200, 6);
            // 
            // miListClear
            // 
            this.miListClear.Enabled = false;
            this.miListClear.Name = "miListClear";
            this.miListClear.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Delete)));
            this.miListClear.Size = new System.Drawing.Size(203, 22);
            this.miListClear.Text = "Clear List";
            this.miListClear.Click += new System.EventHandler(this.MenuClear_Click);
            // 
            // miMonitor
            // 
            this.miMonitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMonitorStart,
            this.miMonitorStop});
            this.miMonitor.Name = "miMonitor";
            this.miMonitor.Size = new System.Drawing.Size(62, 20);
            this.miMonitor.Text = "&Monitor";
            // 
            // miMonitorStart
            // 
            this.miMonitorStart.Enabled = false;
            this.miMonitorStart.Name = "miMonitorStart";
            this.miMonitorStart.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.miMonitorStart.Size = new System.Drawing.Size(152, 22);
            this.miMonitorStart.Text = "&Start";
            this.miMonitorStart.Click += new System.EventHandler(this.MenuStart_Click);
            // 
            // miMonitorStop
            // 
            this.miMonitorStop.Enabled = false;
            this.miMonitorStop.Name = "miMonitorStop";
            this.miMonitorStop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F5)));
            this.miMonitorStop.Size = new System.Drawing.Size(152, 22);
            this.miMonitorStop.Text = "S&top";
            this.miMonitorStop.Click += new System.EventHandler(this.MenuStop_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(205, 376);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "File Operation:";
            // 
            // radMinify
            // 
            this.radMinify.AutoSize = true;
            this.radMinify.Enabled = false;
            this.radMinify.Location = new System.Drawing.Point(391, 395);
            this.radMinify.Name = "radMinify";
            this.radMinify.Size = new System.Drawing.Size(98, 17);
            this.radMinify.TabIndex = 21;
            this.radMinify.TabStop = true;
            this.radMinify.Text = "Minify Seperate";
            this.radMinify.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 454);
            this.Controls.Add(this.txtCombine);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radMinify);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSingle);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.msMainMenu);
            this.Controls.Add(this.radCombMin);
            this.Controls.Add(this.txtSingle);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.radCombine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCombineTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMainMenu;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Combinify";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.cmsListOps.ResumeLayout(false);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.cmsTray.ResumeLayout(false);
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSingle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSingle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCombine;
        private System.Windows.Forms.Button btnCombineTo;
        private System.Windows.Forms.RadioButton radCombMin;
        private System.Windows.Forms.RadioButton radCombine;
        private System.Windows.Forms.ContextMenuStrip cmsListOps;
        private System.Windows.Forms.ToolStripMenuItem smiRemove;
        private System.Windows.Forms.Timer timeCheck;
        private System.Windows.Forms.ToolStripMenuItem smiUp;
        private System.Windows.Forms.ToolStripMenuItem smiDown;
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
        private System.Windows.Forms.ToolStripMenuItem smiExit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miProjectOpen;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miList;
        private System.Windows.Forms.ToolStripMenuItem miListDir;
        private System.Windows.Forms.ToolStripMenuItem miListFile;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miListUp;
        private System.Windows.Forms.ToolStripMenuItem miListDown;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem miListRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem miListClear;
        private System.Windows.Forms.ToolStripMenuItem miFileNew;
        private System.Windows.Forms.ToolStripMenuItem miFileNewFile;
        private System.Windows.Forms.ToolStripMenuItem miFileNewDir;
        private System.Windows.Forms.ToolStripMenuItem miMonitor;
        private System.Windows.Forms.ToolStripMenuItem miMonitorStart;
        private System.Windows.Forms.ToolStripMenuItem miMonitorStop;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem smiClear;
        private System.Windows.Forms.ToolStripMenuItem smiDir;
        private System.Windows.Forms.ToolStripMenuItem smiFile;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radMinify;
    }
}

