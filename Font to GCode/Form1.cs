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
    private FontFamily[] _fontFamilies { get; set; }
    public main()
    {
      InitializeComponent();

      //Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);

      // Allow user to click picturebox
      picCharacter.MouseClick += picCharacter_Clicked;

      _fontFamilies = (new System.Drawing.Text.InstalledFontCollection()).Families;
      ComboBox box = (ComboBox)mnuFontFamilies.Control;
      box.DrawMode = DrawMode.OwnerDrawVariable;
      //box.MeasureItem += mnuFontFamilies_MeasureItem;
      box.DrawItem += mnuFontFamilies_DrawItem;
      mnuFontFamilies.Items.Clear();
      foreach (FontFamily item in _fontFamilies)
      {
        mnuFontFamilies.Items.Add(item.Name);
      }
      mnuFontFamilies.SelectedIndex = mnuFontFamilies.FindString("Arial");

      mnuFontFamilies.SelectedIndexChanged += mnuFontFamilies_SelectedIndexChanged;
      mnuFontFamilies_SelectedIndexChanged(mnuFontFamilies, null);
    }
    
    //private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //  Properties.Settings.Default.Save();
    //}

    private void mnuFontFamilies_DrawItem(object sender, DrawItemEventArgs e)
    {
      try
      {
        Font font = new Font(_fontFamilies[e.Index], ((ComboBox)sender).Font.Size);
        Brush brush = Brushes.Black;
        string text = _fontFamilies[e.Index].Name;
        //e.Graphics.Clear(Color.White);
        e.Graphics.DrawString(text, font, brush, e.Bounds);
      }
      catch (Exception ex)
      {
        statStatus.Text = "Error: " + ex.Message;
      }
    }

    //private void mnuFontFamilies_MeasureItem(object sender, MeasureItemEventArgs e)
    //{
    //  //throw new NotImplementedException();
    //}

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

    private void mnuFontSize_TextChanged(object sender, EventArgs e)
    {
      Properties.Settings.Default.FontSize = mnuFontSize.TextBox.Text;
      Properties.Settings.Default.Save();
      mnuFontFamilies_SelectedIndexChanged(mnuFontFamilies, null);
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
      GC.Collect();
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
      double ratHeight = Convert.ToDouble(Properties.Settings.Default.GCodeHeight);
      double ratWidth = ((imgWidth * ratHeight) / imgHeight);
      var strOut = new StringBuilder();
      strOut.AppendLine("(CHARACTER: " + cp.Character + " HEIGHT: " + ratHeight.ToString("0.0000") + " WIDTH: " + ratWidth.ToString("0.0000") + ")");
      strOut.AppendLine("G91 (BEGIN INCREMENTAL PATH)");
      Point[][] nomPaths = cp.GetPaths();
      Point[][] paths = cp.GetIncrementalPaths();
      int tot = 0;
      for (int i = 0; i < paths.Count(); i++)
      {
        tot += paths[i].Count();
        strOut.AppendLine("(PATH " + (i+1).ToString() + ")");
        for (int j = 0; j < paths[i].Count(); j++)
        {
          double x, y;
          x = (paths[i][j].X / imgWidth) * ratWidth;
          y = (paths[i][j].Y / imgHeight) * ratHeight;
          if (j == 0)
          {
            strOut.AppendLine("G00 X" + x.ToString() + " Y" + y.ToString() + " (GO TO FIRST POINT)");
            strOut.AppendLine("G01 Z-#26 (INCREMENT DOWN TO CUT)");
          }else{
            strOut.AppendLine("X" + x.ToString() + " Y" + y.ToString());
          }
        }
        strOut.AppendLine("G00 Z#26 (INCREMENT OUT)");
        strOut.AppendLine("X" + (((0 - nomPaths[i][nomPaths[i].Count() - 1].X) / imgWidth) * ratWidth).ToString() + " Y-" + (((imgHeight - nomPaths[i][nomPaths[i].Count() - 1].Y) / imgHeight) * ratHeight).ToString() + "(MOVE TO START)");
      }
      //Console.WriteLine("Finished Length=" + tot.ToString());
      strOut.AppendLine("X" + (ratWidth).ToString() + "(MOVE TO NEXT CORNER)");
      strOut.AppendLine("G90 (BACK TO ABSOLUTE FOR SAFETY)");
      return strOut.ToString();
    }
    private string ImageToGCode(AutoEdge ae)
    {
      double imgWidth = ae.Image.Width;
      double imgHeight = ae.Image.Height;
      double ratHeight = Convert.ToDouble(Properties.Settings.Default.GCodeHeight);
      double ratWidth = ((imgWidth * ratHeight) / imgHeight);
      var strOut = new StringBuilder();
      strOut.AppendLine("(IMAGE HEIGHT: " + ratHeight.ToString("0.0000") + "  IMAGE WIDTH: " + ratWidth.ToString("0.0000") + ")");
      strOut.AppendLine("G91 (BEGIN INCREMENTAL PATH)");
      int idx = 0;
      foreach (AutoEdge.Crawler path in ae.EdgeCrawler)
      {
        Point[] nomPts = path.GetPointPath();
        Point[] paths = path.GetPointPath_Incremental(ae.Image.Size);

        strOut.AppendLine("(PATH " + (idx + 1).ToString() + ")");
        for (int j = 0; j < paths.Count(); j++)
        {
          double x, y;
          x = (paths[j].X / imgWidth) * ratWidth;
          y = (paths[j].Y / imgHeight) * ratHeight;
          if (j == 0)
          {
            strOut.AppendLine("G00 X" + x.ToString() + " Y" + y.ToString() + " (GO TO FIRST POINT)");
            strOut.AppendLine("G01 Z-#26 (INCREMENT DOWN TO CUT)");
          }
          else
          {
            strOut.AppendLine("X" + x.ToString() + " Y" + y.ToString());
          }
        }
        strOut.AppendLine("G00 Z#26 (INCREMENT OUT)");
        strOut.AppendLine("X" + (((0 - nomPts[nomPts.Count() - 1].X) / imgWidth) * ratWidth).ToString() + " Y-" + (((imgHeight - nomPts[nomPts.Count() - 1].Y) / imgHeight) * ratHeight).ToString() + "(MOVE TO START)");
        idx += 1;
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
      var strOut = new StringBuilder();
      strOut.AppendLine("O" + Properties.Settings.Default.GCodeProgram);
      strOut.AppendLine("(MACRO: " + trvCharacters.SelectedNode.Text + " - " + mnuFontFamilies.SelectedItem.ToString() + " FONT " + Properties.Settings.Default.FontSize + "em)");
      strOut.AppendLine();
      strOut.AppendLine("(SHORTEST DISTANCE: " + ((CharacterPath)trvCharacters.SelectedNode.Tag).EdgeDetector.GetShortestDistance().ToString() + ")");
      strOut.AppendLine();
      strOut.AppendLine("(-- USAGE --)");
      strOut.AppendLine("( - USE G65 TO CALL THIS PROGRAM)");
      strOut.AppendLine("(EXAMPLES - G65 P" + Properties.Settings.Default.GCodeProgram + " Z0.2 S1 (F 0.2 DEEP 1:1 SCALE))");
      strOut.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + " (F 0.1 DEEP 1:1 SCALE))");
      strOut.AppendLine();
      strOut.AppendLine("(DATE: " + DateTime.Now.ToString("yyyy-MM-dd hh-mm tt") + ")");
      strOut.AppendLine();
      strOut.AppendLine("(-- VARIABLE DEFINITIONS --)");
      strOut.AppendLine("([S]#19 = SCALE PATTERN IN XY)");
      strOut.AppendLine("([Z]#26 = DEPTH OF CUT)");
      strOut.AppendLine();
      strOut.AppendLine("IF[#19NE0]GOTO10 (SCALE PASSED IN MACRO CALL)");
      strOut.AppendLine("#19 = 1 (SET DEFAULT SCALE)");
      strOut.AppendLine("N10 (SKIPPED DEFAULT SCALE)");
      strOut.AppendLine("IF[#26NE0]GOTO11 (DEPTH PASSED IN MACRO CALL)");
      strOut.AppendLine("#26 = " + Convert.ToDouble(Properties.Settings.Default.GCodeDepth).ToString() + " (SET DEFAULT DEPTH)");
      strOut.AppendLine("N11 (SKIPPED DEFAULT DEPTH)");
      strOut.AppendLine();
      strOut.AppendLine(CharacterToGCode((CharacterPath)trvCharacters.SelectedNode.Tag));
      // Save file out
      System.IO.File.WriteAllText(sv.FileName, strOut.ToString());
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
      int idx = 1;
      var strHead = new StringBuilder();
      var strMacros = new StringBuilder();

      strHead.AppendLine("O" + Properties.Settings.Default.GCodeProgram);
      strHead.AppendLine("(MACRO: " + mnuFontFamilies.SelectedItem.ToString() + " FONT " + Properties.Settings.Default.FontSize + "em)");
      strHead.AppendLine("(DATE: " + DateTime.Now.ToString("yyyy-MM-dd hh-mm tt") + ")");
      strHead.AppendLine();
      strHead.AppendLine("(SHORTEST DISTANCE: {DISTANCE}in)");
      strHead.AppendLine();
      strHead.AppendLine("(-- USAGE --)");
      strHead.AppendLine("( - USE G65 TO CALL THIS PROGRAM)");
      strHead.AppendLine("( - PASS ARGUMENTS -SEE BELOW-, AT LEAST CHARACTER INDEX)");
      strHead.AppendLine("( - REFERENCE THE LIST BELOW FOR CHARACTER INDICES)");
      strHead.AppendLine("(EXAMPLES - G65 P" + Properties.Settings.Default.GCodeProgram + " C70 Z0.1 S1 (F 0.1 DEEP 1:1 SCALE))");
      strHead.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + " C79 Z0.2 S1 (O 0.2 DEEP 1:1 SCALE))");
      strHead.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + " C78 S1      (N 0.1 DEEP 1:1 SCALE))");
      strHead.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + " C84         (T 0.1 DEEP 1:1 SCALE))");
      strHead.AppendLine();
      strHead.AppendLine("(-- VARIABLE DEFINITIONS --)");
      strHead.AppendLine("([C]#3  = CHARACTER INDEX -SEE BELOW-)");
      strHead.AppendLine("([S]#19 = SCALE PATTERN IN XY)");
      strHead.AppendLine("([Z]#26 = DEPTH OF CUT)");
      strHead.AppendLine();
      strHead.AppendLine("IF[#19NE0]GOTO10 (SCALE PASSED IN MACRO CALL)");
      strHead.AppendLine("#19 = 1 (SET DEFAULT SCALE)");
      strHead.AppendLine("N10 (SKIPPED DEFAULT SCALE)");
      strHead.AppendLine("IF[#26NE0]GOTO11 (DEPTH PASSED IN MACRO CALL)");
      strHead.AppendLine("#26 = " + Convert.ToDouble(Properties.Settings.Default.GCodeDepth).ToString() + " (SET DEFAULT DEPTH)");
      strHead.AppendLine("N11 (SKIPPED DEFAULT DEPTH)");
      strHead.AppendLine();
      strHead.AppendLine("(-- CHARACTER INDICES --)");

      double dist = 0;
      double shortest = 999;
      double avgHeight = 0;
      foreach (TreeNode item in trvCharacters.Nodes[0].Nodes)
      {
        strHead.AppendLine("(" + ((int)item.Text[0]).ToString() + " = " + item.Text + ")");
        strMacros.AppendLine("IF[#3EQ" + ((int)item.Text[0]).ToString() + "]H" + ((int)item.Text[0]).ToString() + " (CALL MACRO FOR " + item.Text + ")");
        strOut.AppendLine("N" + ((int)item.Text[0]).ToString() + " (BEGIN CHARACTER: " + item.Text + ")");
        strOut.AppendLine(Properties.Settings.Default.GCodeInit + " (INITIALIZE)");

        trvCharacters.SelectedNode = item;
        trvCharacters_AfterSelect(trvCharacters, null);
        CharacterPath cp = (CharacterPath)trvCharacters.SelectedNode.Tag;
        strOut.AppendLine(CharacterToGCode(cp));
        dist = (cp).EdgeDetector.GetShortestDistance();
        if (dist < shortest)
          shortest = dist;
        avgHeight += cp.Image.Height;
        strOut.AppendLine("M99");
        idx += 1;
      }
      avgHeight /= idx;
      strHead.Replace("{DISTANCE}", ((shortest / avgHeight) * Convert.ToDouble(Properties.Settings.Default.GCodeHeight)).ToString("0.0000"));
      strHead.AppendLine();
      strHead.AppendLine(strMacros.ToString());
      strHead.AppendLine("GOTO200 (SKIP ALL SUBS)");
      strOut.Insert(0, strHead);
      strOut.AppendLine("N200 (SKIPPED SUBS)");
      strOut.AppendLine("M99 (END)");

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
      List<string> lstChars = new List<string>();
      Cursor = Cursors.WaitCursor;
      SaveFileDialog sv = (SaveFileDialog)sender;
      string input = Microsoft.VisualBasic.Interaction.InputBox("Text To G-Code", "Enter text to convert to incremental G-Code:");
      if (!String.IsNullOrEmpty(input))
      {
        FontToPathModel ftp = new FontToPathModel(mnuFontFamilies.SelectedItem.ToString(), input.ToCharArray().Select(c=>c.ToString()).ToArray());
        var strOut = new StringBuilder();
        var strHead = new StringBuilder();
        var strMacros = new StringBuilder();
        strHead.AppendLine("O" + Properties.Settings.Default.GCodeProgram);
        strHead.AppendLine("(MACRO: " + mnuFontFamilies.SelectedItem.ToString() + " - " +  input + " " + Properties.Settings.Default.FontSize + "em)");
        strHead.AppendLine("(DATE: " + DateTime.Now.ToString("yyyy-MM-dd hh-mm tt") + ")");
        strHead.AppendLine();
        strHead.AppendLine("(-- USAGE --)");
        strHead.AppendLine("( - USE G65 TO CALL THIS PROGRAM)");
        strHead.AppendLine("(EXAMPLES - G65 P" + Properties.Settings.Default.GCodeProgram + " Z0.1 S1 (" + input + " 0.1 DEEP 1:1 SCALE))");
        strHead.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + " Z0.2 S1 (" + input + " 0.2 DEEP 1:1 SCALE))");
        strHead.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + " S1      (" + input + " 0.1 DEEP 1:1 SCALE))");
        strHead.AppendLine("(           G65 P" + Properties.Settings.Default.GCodeProgram + "         (" + input + " 0.1 DEEP 1:1 SCALE))");
        strHead.AppendLine();
        strHead.AppendLine("(-- VARIABLE DEFINITIONS --)");
        strHead.AppendLine("([S]#19 = SCALE PATTERN IN XY)");
        strHead.AppendLine("([Z]#26 = DEPTH OF CUT)");
        strHead.AppendLine();
        strHead.AppendLine("IF[#19NE0]GOTO10 (SCALE PASSED IN MACRO CALL)");
        strHead.AppendLine("#19 = 1 (SET DEFAULT SCALE)");
        strHead.AppendLine("N10 (SKIPPED DEFAULT SCALE)");
        strHead.AppendLine("IF[#26NE0]GOTO11 (DEPTH PASSED IN MACRO CALL)");
        strHead.AppendLine("#26 = " + Convert.ToDouble(Properties.Settings.Default.GCodeDepth).ToString() + " (SET DEFAULT DEPTH)");
        strHead.AppendLine("N11 (SKIPPED DEFAULT DEPTH)");
        foreach (CharacterPath cp in ftp.CharacterLibrary)
        {
          if (!lstChars.Contains(cp.Character))
          {
            lstChars.Add(cp.Character);
            strHead.Append("(" + ((int)cp.Character[0]).ToString());
            strHead.AppendFormat("{0:5}\r\n", " = " + cp.Character + ")");
            cp.Draw(true);
            strOut.AppendLine("N" + ((int)cp.Character[0]).ToString() + " (BEGIN CHARACTER " + cp.Character + ")");
            strOut.AppendLine(Properties.Settings.Default.GCodeInit + " (INITIALIZE)");
            strOut.AppendLine(CharacterToGCode(cp));
            strOut.AppendLine("M99");
          }
          strMacros.Append("M98 H" + ((int)cp.Character[0]).ToString());
          strMacros.AppendFormat("{0:10}\r\n", "(" + cp.Character + ")");
        }
        strHead.AppendLine(strMacros.ToString());
        strHead.AppendLine("GOTO200 (SKIP ALL SUBS)");
        strOut.Insert(0, strHead);
        strOut.AppendLine("N200 (SKIPPED SUBS)");
        strOut.AppendLine("M99 (END)");

        // Save file out
        System.IO.File.WriteAllText(sv.FileName, strOut.ToString());
        statStatus.Text = "Saved!";
      }else
      {
        statStatus.Text = "String cannot be empty!";
      }
      Cursor = Cursors.Default;
    }

    private void mnuCharacters_Click(object sender, EventArgs e)
    {
      Settings_Characters swin = new Settings_Characters();
      if (swin.ShowDialog() == DialogResult.OK)
      {
        mnuFontFamilies_SelectedIndexChanged(mnuFontFamilies, null);
        swin.Close();
        swin.Dispose();
      }
    }

    private void mnuSettingsGCode_Click(object sender, EventArgs e)
    {
      Settings_GCode swin = new Settings_GCode();
      if (swin.ShowDialog() == DialogResult.OK)
      {
        mnuFontFamilies_SelectedIndexChanged(mnuFontFamilies, null);
        swin.Close();
        swin.Dispose();
      }
    }

    private void mnuSaveImageToGCode_Click(object sender, EventArgs e)
    {
      OpenFileDialog opn = new OpenFileDialog();
      opn.Title = "Select an image...";
      opn.Filter = "Bitmap (*.bmp)|*.bmp";
      DialogResult dr = opn.ShowDialog();
      if (dr != DialogResult.None && dr != DialogResult.Cancel && System.IO.File.Exists(opn.FileName))
      {
        Bitmap bmp = new Bitmap(opn.FileName);
        AutoEdge ae = new AutoEdge(bmp);
        trvCharacters.SelectedNode = trvCharacters.Nodes[0];
        picCharacter.Image = ae.Image;
        SaveFileDialog sv = new SaveFileDialog();
        sv.CheckPathExists = true;
        sv.OverwritePrompt = true;
        sv.Filter = "EIA (*.eia)|.eia|G-Code (*.gcode)|*.gcode";
        dr = sv.ShowDialog();
        if (dr != DialogResult.None && dr != DialogResult.Cancel)
        {
          var strOut = new StringBuilder();
          strOut.AppendLine("O" + Properties.Settings.Default.GCodeProgram);
          strOut.AppendLine("(MACRO: STENCIL " + opn.FileName.Remove(0, opn.FileName.LastIndexOf("\\") + 1) + ")");
          strOut.AppendLine("(DATETIME: " + DateTime.Now.ToString("yyyy-MM-dd hh-mm tt") + ")");
          strOut.AppendLine(Properties.Settings.Default.GCodeInit + " (INITIALIZE)");
          strOut.AppendLine(ImageToGCode(ae));
          strOut.AppendLine("M99");
          System.IO.File.WriteAllText(sv.FileName, strOut.ToString());
          statStatus.Text = "Saved!";
        }
      }
    }
    
  }
}
