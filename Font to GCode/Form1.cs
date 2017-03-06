using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Font_to_GCode
{
  public partial class main : Form
  {
    private FontToPathModel Project { get; set; }

    public main()
    {
      InitializeComponent();

      // Allow user to click picturebox
      picCharacter.MouseClick += picCharacter_Clicked;

      FontFamily[] fontFamilies = (new System.Drawing.Text.InstalledFontCollection()).Families;
      mnuFontFamilies.Items.Clear();
      foreach (FontFamily item in fontFamilies)
      {
        mnuFontFamilies.Items.Add(item.Name);
      }
      mnuFontFamilies.SelectedIndex = mnuFontFamilies.FindString("Arial");

      mnuFontFamilies.SelectedIndexChanged += mnuFontFamilies_SelectedIndexChanged;
      mnuFontFamilies_SelectedIndexChanged(mnuFontFamilies, null);
    }

    private void mnuFontFamilies_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        statFontFile.Text = mnuFontFamilies.SelectedItem.ToString();
        string[] strArr = new string[Properties.Settings.Default.CharacterList.Count];
        Properties.Settings.Default.CharacterList.CopyTo(strArr, 0);
        Project = new FontToPathModel(statFontFile.Text, strArr);
        // Clear TreeView
        trvCharacters.Nodes.Clear();
        TreeNode tRoot = trvCharacters.Nodes.Add("Characters");
        foreach (CharacterPath item in Project.CharacterLibrary)
        {
          tRoot.Nodes.Add(item.Character).Tag = item;
        }
        trvCharacters.ExpandAll();
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }


    
    private void picCharacter_Clicked(object sender, MouseEventArgs e)
    {
      //Console.WriteLine("{Width: " + picCharacter.Size.Width.ToString() + ", Height: " + picCharacter.Size.Height.ToString() + ", X: " + e.X.ToString() + ", Y: " + e.Y.ToString() + ", PercentX: " + (((double)e.X / (double)picCharacter.Size.Width)).ToString() + ", PercentY: " + (((double)e.Y / (double)picCharacter.Height)).ToString() + "}");
      //if (trvCharacters.SelectedNode != null && trvCharacters.SelectedNode.Tag != null)
      //{
      //  CharacterPath cp = (CharacterPath)trvCharacters.SelectedNode.Tag;

      //  statStatus.Text = "Added ImagePoint: " + cp.Add(e.X,e.Y,picCharacter.Size).ToString();
      //  CharacterPath_Updated(cp);
      //}
    }

    private void CharacterPath_Updated(CharacterPath cp)
    {
      Cursor = Cursors.WaitCursor;
      //picCharacter.Image = cp.Draw( true);
      picCharacter.Image = cp.Draw(true);
      if (cp.EdgeDetector != null)
      {
        statPointCount.Text = cp.EdgeDetector.EdgeCrawler.Count().ToString() + " Paths";
      }
      Cursor = Cursors.Default;
    }

    private void trvCharacters_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (trvCharacters.SelectedNode != null && trvCharacters.SelectedNode.Tag != null)
      {
        CharacterPath cp = (CharacterPath)trvCharacters.SelectedNode.Tag;
        CharacterPath_Updated(cp);
      }
    }
    
    private string CharacterToGCode(CharacterPath cp)
    {
      double imgWidth = cp.Image.Width;
      double imgHeight = cp.Image.Height;
      double ratHeight = Convert.ToDouble(txtActualHeight.Text);
      double ratWidth = ((imgWidth * ratHeight) / imgHeight);
      var strOut = new StringBuilder();
      strOut.AppendLine("(CHARACTER: " + cp.Character + ")");
      strOut.AppendLine("G91 (BEGIN INCREMENTAL PATH)");
      Point[][] nomPaths = cp.GetPaths();
      Point[][] paths = cp.GetIncrementalPaths();
      for (int i = 0; i < paths.Count(); i++)
      {
        strOut.AppendLine("(PATH " + (i+1).ToString() + ")");
        for (int j = 0; j < paths[i].Count(); j++)
        {
          double x, y;
          x = (paths[i][j].X / imgWidth) * ratWidth;//(paths[i][j].X * ratio);
          y = (paths[i][j].Y / imgHeight) * ratHeight;//(paths[i][j].Y * ratio);
          if (j == 0)
          {
            strOut.AppendLine("X" + x.ToString() + " Y" + y.ToString() + " (GO TO FIRST POINT)");
            strOut.AppendLine("Z-" + Convert.ToDouble(txtActualDepth.Text).ToString() + " (INCREMENT DOWN TO CUT)");
          }else{
            strOut.AppendLine("X" + x.ToString() + " Y" + y.ToString());
          }
        }
        strOut.AppendLine("Z" + Convert.ToDouble(txtActualDepth.Text).ToString() + " (INCREMENT OUT)");
        strOut.AppendLine("X" + (((0 - nomPaths[i][nomPaths[i].Count() - 1].X) / imgWidth) * ratWidth).ToString() + " Y-" + (((imgHeight - nomPaths[i][nomPaths[i].Count() - 1].Y) / imgHeight) * ratHeight).ToString() + "(MOVE TO START)");
      }
      strOut.AppendLine("X" + (ratWidth).ToString() + "(MOVE TO NEXT CORNER)");
      strOut.AppendLine("G90 (BACK TO ABSOLUTE FOR SAFETY)");
      return strOut.ToString();
    }

    private void mnuSaveCharacterPath_Click(object sender, EventArgs e)
    {
      if (trvCharacters.SelectedNode != null)
      {
        SaveFileDialog sv = new SaveFileDialog();
        sv.CheckPathExists = true;
        sv.OverwritePrompt = true;
        sv.FileOk += Sv_FileOk_Character;
        sv.FileName = mnuFontFamilies.SelectedItem.ToString() + "_" + trvCharacters.SelectedNode.Text;
        sv.Filter = "EIA (*.eia)|.eia|G-Code (*.gcode)|*.gcode";
        sv.ShowDialog();
      }else
      {
        statStatus.Text = "Must select a character from the list!";
      }
    }
    private void Sv_FileOk_Character(object sender, CancelEventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      SaveFileDialog sv = (SaveFileDialog)sender;
      
      // Save file out
      System.IO.File.WriteAllText(sv.FileName, CharacterToGCode((CharacterPath)trvCharacters.SelectedNode.Tag));
      statStatus.Text = "Saved!";
      Cursor = Cursors.Default;
    }

    private void mnuSaveAllCharacterPaths_Click(object sender, EventArgs e)
    {
      SaveFileDialog sv = new SaveFileDialog();
      sv.CheckPathExists = true;
      sv.OverwritePrompt = true;
      sv.FileOk += Sv_FileOk_AllCharacters; ;
      sv.FileName = mnuFontFamilies.SelectedItem.ToString();
      sv.Filter = "EIA (*.eia)|.eia|G-Code (*.gcode)|*.gcode";
      sv.ShowDialog();
    }

    private void Sv_FileOk_AllCharacters(object sender, CancelEventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      SaveFileDialog sv = (SaveFileDialog)sender;
      var strOut = new StringBuilder();
      foreach (TreeNode item in trvCharacters.Nodes[0].Nodes)
      {
        trvCharacters.SelectedNode = item;
        trvCharacters_AfterSelect(trvCharacters, null);
        strOut.AppendLine(CharacterToGCode((CharacterPath)trvCharacters.SelectedNode.Tag));
      }

      // Save file out
      System.IO.File.WriteAllText(sv.FileName, strOut.ToString());
      statStatus.Text = "Saved!";
      Cursor = Cursors.Default;
    }
    
    private void textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
      ToolStripTextBox txt = (ToolStripTextBox)sender;
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
         (e.KeyChar != '.'))
      {
        e.Handled = true;
      }

      // only allow one decimal point
      if ((e.KeyChar == '.') && (txt.Text.IndexOf('.') > -1))
      {
        e.Handled = true;
      }
    }

    private void mnuSaveInputText_Click(object sender, EventArgs e)
    {
      SaveFileDialog sv = new SaveFileDialog();
      sv.CheckPathExists = true;
      sv.OverwritePrompt = true;
      sv.FileOk += Sv_FileOk_InputText; ;
      sv.Filter = "EIA (*.eia)|.eia|G-Code (*.gcode)|*.gcode";
      sv.ShowDialog();
    }

    private void Sv_FileOk_InputText(object sender, CancelEventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      SaveFileDialog sv = (SaveFileDialog)sender;
      string input = Microsoft.VisualBasic.Interaction.InputBox("Text To G-Code", "Enter text to convert to incremental G-Code:");
      if (!String.IsNullOrEmpty(input))
      {
        FontToPathModel ftp = new FontToPathModel(mnuFontFamilies.SelectedItem.ToString(), input.ToCharArray().Select(c=>c.ToString()).ToArray());
        var strOut = new StringBuilder();
        foreach (CharacterPath cp in ftp.CharacterLibrary)
        {
          cp.Draw(true);
          strOut.AppendLine(CharacterToGCode(cp));
        }

        // Save file out
        System.IO.File.WriteAllText(sv.FileName, strOut.ToString());
        statStatus.Text = "Saved!";
      }else
      {
        statStatus.Text = "String cannot be empty!";
      }
      Cursor = Cursors.Default;
    }
  }
}
