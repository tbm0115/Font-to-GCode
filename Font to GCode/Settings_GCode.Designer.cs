namespace Font_to_GCode
{
  partial class Settings_GCode
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings_GCode));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtHeight = new System.Windows.Forms.TextBox();
      this.txtDepth = new System.Windows.Forms.TextBox();
      this.txtProgram = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtInit = new System.Windows.Forms.TextBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(142, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Default Height (inch):";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(17, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(139, 17);
      this.label2.TabIndex = 1;
      this.label2.Text = "Default Depth (inch):";
      // 
      // txtHeight
      // 
      this.txtHeight.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Font_to_GCode.Properties.Settings.Default, "GCodeHeight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.txtHeight.Location = new System.Drawing.Point(162, 13);
      this.txtHeight.Name = "txtHeight";
      this.txtHeight.Size = new System.Drawing.Size(100, 22);
      this.txtHeight.TabIndex = 2;
      this.txtHeight.Text = global::Font_to_GCode.Properties.Settings.Default.GCodeHeight;
      this.txtHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress);
      // 
      // txtDepth
      // 
      this.txtDepth.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Font_to_GCode.Properties.Settings.Default, "GCodeDepth", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.txtDepth.Location = new System.Drawing.Point(162, 41);
      this.txtDepth.Name = "txtDepth";
      this.txtDepth.Size = new System.Drawing.Size(100, 22);
      this.txtDepth.TabIndex = 3;
      this.txtDepth.Text = global::Font_to_GCode.Properties.Settings.Default.GCodeDepth;
      this.txtDepth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress);
      // 
      // txtProgram
      // 
      this.txtProgram.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Font_to_GCode.Properties.Settings.Default, "GCodeProgram", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.txtProgram.Location = new System.Drawing.Point(162, 69);
      this.txtProgram.Name = "txtProgram";
      this.txtProgram.Size = new System.Drawing.Size(100, 22);
      this.txtProgram.TabIndex = 5;
      this.txtProgram.Text = global::Font_to_GCode.Properties.Settings.Default.GCodeProgram;
      this.txtProgram.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(77, 72);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(78, 17);
      this.label3.TabIndex = 4;
      this.label3.Text = "Program #:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 105);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(116, 17);
      this.label4.TabIndex = 6;
      this.label4.Text = "Initialization Line:";
      // 
      // txtInit
      // 
      this.txtInit.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Font_to_GCode.Properties.Settings.Default, "GCodeInit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.txtInit.Location = new System.Drawing.Point(12, 125);
      this.txtInit.Name = "txtInit";
      this.txtInit.Size = new System.Drawing.Size(250, 22);
      this.txtInit.TabIndex = 7;
      this.txtInit.Text = global::Font_to_GCode.Properties.Settings.Default.GCodeInit;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(103, 166);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 33);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // Settings_GCode
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(279, 209);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.txtInit);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.txtProgram);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.txtDepth);
      this.Controls.Add(this.txtHeight);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Settings_GCode";
      this.Text = "Settings: G-Code";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtHeight;
    private System.Windows.Forms.TextBox txtDepth;
    private System.Windows.Forms.TextBox txtProgram;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtInit;
    private System.Windows.Forms.Button btnOK;
  }
}