namespace Font_to_GCode
{
  partial class main
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
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuSaveCharacterPath = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuSaveAllCharacterPaths = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuToolDefault = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuToolPicker = new System.Windows.Forms.ToolStripMenuItem();
      this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuCharacters = new System.Windows.Forms.ToolStripMenuItem();
      this.setHeightinchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.txtActualHeight = new System.Windows.Forms.ToolStripTextBox();
      this.setDepthinchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.txtActualDepth = new System.Windows.Forms.ToolStripTextBox();
      this.mnuFontFamilies = new System.Windows.Forms.ToolStripComboBox();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.statFontFile = new System.Windows.Forms.ToolStripStatusLabel();
      this.statStatus = new System.Windows.Forms.ToolStripStatusLabel();
      this.statPointCount = new System.Windows.Forms.ToolStripStatusLabel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.trvCharacters = new System.Windows.Forms.TreeView();
      this.picCharacter = new System.Windows.Forms.PictureBox();
      this.mnuSaveInputText = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).BeginInit();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.mnuFontFamilies});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(825, 32);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.toolStripSeparator2,
            this.mnuQuit});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 28);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // mnuSave
      // 
      this.mnuSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveCharacterPath,
            this.mnuSaveAllCharacterPaths,
            this.mnuSaveInputText});
      this.mnuSave.Name = "mnuSave";
      this.mnuSave.Size = new System.Drawing.Size(181, 26);
      this.mnuSave.Text = "Save G-Code...";
      // 
      // mnuSaveCharacterPath
      // 
      this.mnuSaveCharacterPath.Name = "mnuSaveCharacterPath";
      this.mnuSaveCharacterPath.Size = new System.Drawing.Size(199, 26);
      this.mnuSaveCharacterPath.Text = "Current Character";
      this.mnuSaveCharacterPath.Click += new System.EventHandler(this.mnuSaveCharacterPath_Click);
      // 
      // mnuSaveAllCharacterPaths
      // 
      this.mnuSaveAllCharacterPaths.Name = "mnuSaveAllCharacterPaths";
      this.mnuSaveAllCharacterPaths.Size = new System.Drawing.Size(199, 26);
      this.mnuSaveAllCharacterPaths.Text = "All Characters";
      this.mnuSaveAllCharacterPaths.Click += new System.EventHandler(this.mnuSaveAllCharacterPaths_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
      // 
      // mnuQuit
      // 
      this.mnuQuit.Name = "mnuQuit";
      this.mnuQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
      this.mnuQuit.Size = new System.Drawing.Size(181, 26);
      this.mnuQuit.Text = "Quit";
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolDefault,
            this.mnuToolPicker});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 28);
      this.toolsToolStripMenuItem.Text = "Tools";
      this.toolsToolStripMenuItem.Visible = false;
      // 
      // mnuToolDefault
      // 
      this.mnuToolDefault.Name = "mnuToolDefault";
      this.mnuToolDefault.Size = new System.Drawing.Size(133, 26);
      this.mnuToolDefault.Text = "Default";
      // 
      // mnuToolPicker
      // 
      this.mnuToolPicker.Name = "mnuToolPicker";
      this.mnuToolPicker.Size = new System.Drawing.Size(133, 26);
      this.mnuToolPicker.Text = "Picker";
      // 
      // settingsToolStripMenuItem
      // 
      this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCharacters,
            this.setHeightinchToolStripMenuItem,
            this.setDepthinchToolStripMenuItem});
      this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
      this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 28);
      this.settingsToolStripMenuItem.Text = "Settings";
      // 
      // mnuCharacters
      // 
      this.mnuCharacters.Name = "mnuCharacters";
      this.mnuCharacters.Size = new System.Drawing.Size(222, 26);
      this.mnuCharacters.Text = "Configure Characters";
      this.mnuCharacters.Click += new System.EventHandler(this.mnuCharacters_Click);
      // 
      // setHeightinchToolStripMenuItem
      // 
      this.setHeightinchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtActualHeight});
      this.setHeightinchToolStripMenuItem.Name = "setHeightinchToolStripMenuItem";
      this.setHeightinchToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
      this.setHeightinchToolStripMenuItem.Text = "Set Height (inch)";
      // 
      // txtActualHeight
      // 
      this.txtActualHeight.Name = "txtActualHeight";
      this.txtActualHeight.Size = new System.Drawing.Size(100, 27);
      this.txtActualHeight.Text = "0.5";
      this.txtActualHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress);
      // 
      // setDepthinchToolStripMenuItem
      // 
      this.setDepthinchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtActualDepth});
      this.setDepthinchToolStripMenuItem.Name = "setDepthinchToolStripMenuItem";
      this.setDepthinchToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
      this.setDepthinchToolStripMenuItem.Text = "Set Depth (inch)";
      // 
      // txtActualDepth
      // 
      this.txtActualDepth.Name = "txtActualDepth";
      this.txtActualDepth.Size = new System.Drawing.Size(100, 27);
      this.txtActualDepth.Text = "0.1";
      this.txtActualDepth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress);
      // 
      // mnuFontFamilies
      // 
      this.mnuFontFamilies.AutoSize = false;
      this.mnuFontFamilies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mnuFontFamilies.Name = "mnuFontFamilies";
      this.mnuFontFamilies.Size = new System.Drawing.Size(200, 28);
      // 
      // statusStrip1
      // 
      this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statFontFile,
            this.statStatus,
            this.statPointCount});
      this.statusStrip1.Location = new System.Drawing.Point(0, 408);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(825, 25);
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // statFontFile
      // 
      this.statFontFile.AutoSize = false;
      this.statFontFile.AutoToolTip = true;
      this.statFontFile.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.statFontFile.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
      this.statFontFile.Name = "statFontFile";
      this.statFontFile.Size = new System.Drawing.Size(410, 20);
      this.statFontFile.Spring = true;
      this.statFontFile.Text = "Font File";
      this.statFontFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // statStatus
      // 
      this.statStatus.AutoSize = false;
      this.statStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.statStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
      this.statStatus.Name = "statStatus";
      this.statStatus.Size = new System.Drawing.Size(300, 20);
      this.statStatus.Text = "Action";
      // 
      // statPointCount
      // 
      this.statPointCount.AutoSize = false;
      this.statPointCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.statPointCount.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
      this.statPointCount.Name = "statPointCount";
      this.statPointCount.Size = new System.Drawing.Size(100, 20);
      this.statPointCount.Text = "Point Count";
      // 
      // splitContainer1
      // 
      this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 32);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.trvCharacters);
      this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(5);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.picCharacter);
      this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5);
      this.splitContainer1.Size = new System.Drawing.Size(825, 376);
      this.splitContainer1.SplitterDistance = 273;
      this.splitContainer1.SplitterWidth = 7;
      this.splitContainer1.TabIndex = 2;
      // 
      // trvCharacters
      // 
      this.trvCharacters.BackColor = System.Drawing.SystemColors.Control;
      this.trvCharacters.Dock = System.Windows.Forms.DockStyle.Fill;
      this.trvCharacters.HideSelection = false;
      this.trvCharacters.Location = new System.Drawing.Point(5, 5);
      this.trvCharacters.Name = "trvCharacters";
      this.trvCharacters.Size = new System.Drawing.Size(259, 362);
      this.trvCharacters.TabIndex = 0;
      this.trvCharacters.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvCharacters_AfterSelect);
      // 
      // picCharacter
      // 
      this.picCharacter.BackColor = System.Drawing.Color.White;
      this.picCharacter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.picCharacter.Location = new System.Drawing.Point(5, 5);
      this.picCharacter.Name = "picCharacter";
      this.picCharacter.Size = new System.Drawing.Size(531, 362);
      this.picCharacter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.picCharacter.TabIndex = 0;
      this.picCharacter.TabStop = false;
      // 
      // mnuSaveInputText
      // 
      this.mnuSaveInputText.Name = "mnuSaveInputText";
      this.mnuSaveInputText.Size = new System.Drawing.Size(199, 26);
      this.mnuSaveInputText.Text = "Input Text";
      this.mnuSaveInputText.Click += new System.EventHandler(this.mnuSaveInputText_Click);
      // 
      // main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(825, 433);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "main";
      this.Text = "Font to G-Code";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripMenuItem mnuSave;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem mnuQuit;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mnuCharacters;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ToolStripStatusLabel statFontFile;
    private System.Windows.Forms.ToolStripStatusLabel statStatus;
    private System.Windows.Forms.TreeView trvCharacters;
    private System.Windows.Forms.PictureBox picCharacter;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mnuToolDefault;
    private System.Windows.Forms.ToolStripMenuItem mnuToolPicker;
    private System.Windows.Forms.ToolStripStatusLabel statPointCount;
    private System.Windows.Forms.ToolStripComboBox mnuFontFamilies;
    private System.Windows.Forms.ToolStripMenuItem mnuSaveCharacterPath;
    private System.Windows.Forms.ToolStripMenuItem mnuSaveAllCharacterPaths;
    private System.Windows.Forms.ToolStripMenuItem setHeightinchToolStripMenuItem;
    private System.Windows.Forms.ToolStripTextBox txtActualHeight;
    private System.Windows.Forms.ToolStripMenuItem setDepthinchToolStripMenuItem;
    private System.Windows.Forms.ToolStripTextBox txtActualDepth;
    private System.Windows.Forms.ToolStripMenuItem mnuSaveInputText;
  }
}

