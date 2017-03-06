namespace Font_to_GCode
{
  partial class Settings_Characters
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
      this.label1 = new System.Windows.Forms.Label();
      this.txtNewCharacter = new System.Windows.Forms.TextBox();
      this.lstCharacters = new System.Windows.Forms.ListBox();
      this.btnAdd = new System.Windows.Forms.Button();
      this.btnRemove = new System.Windows.Forms.Button();
      this.btnOk = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(143, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enter New Character:";
      // 
      // txtNewCharacter
      // 
      this.txtNewCharacter.Location = new System.Drawing.Point(12, 33);
      this.txtNewCharacter.MaxLength = 1;
      this.txtNewCharacter.Name = "txtNewCharacter";
      this.txtNewCharacter.Size = new System.Drawing.Size(162, 22);
      this.txtNewCharacter.TabIndex = 0;
      // 
      // lstCharacters
      // 
      this.lstCharacters.FormattingEnabled = true;
      this.lstCharacters.ItemHeight = 16;
      this.lstCharacters.Location = new System.Drawing.Point(12, 73);
      this.lstCharacters.Name = "lstCharacters";
      this.lstCharacters.Size = new System.Drawing.Size(243, 164);
      this.lstCharacters.TabIndex = 2;
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(180, 33);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(75, 23);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // btnRemove
      // 
      this.btnRemove.Location = new System.Drawing.Point(180, 243);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(75, 23);
      this.btnRemove.TabIndex = 3;
      this.btnRemove.Text = "Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // btnOk
      // 
      this.btnOk.Location = new System.Drawing.Point(87, 281);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(87, 34);
      this.btnOk.TabIndex = 4;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // Settings_Characters
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(265, 319);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.btnRemove);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.lstCharacters);
      this.Controls.Add(this.txtNewCharacter);
      this.Controls.Add(this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Settings_Characters";
      this.Text = "Settings: Default Characters";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtNewCharacter;
    private System.Windows.Forms.ListBox lstCharacters;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnOk;
  }
}