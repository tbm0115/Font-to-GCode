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


    
    private void picCharacter_Clicked(object sender, MouseEventArgs e)
    {
      Console.WriteLine("{Width: " + picCharacter.Size.Width.ToString() + ", Height: " + picCharacter.Size.Height.ToString() + ", X: " + e.X.ToString() + ", Y: " + e.Y.ToString() + ", PercentX: " + (((double)e.X / (double)picCharacter.Size.Width)).ToString() + ", PercentY: " + (((double)e.Y / (double)picCharacter.Height)).ToString() + "}");
      if (trvCharacters.SelectedNode != null && trvCharacters.SelectedNode.Tag != null)
      {
        CharacterPath cp = (CharacterPath)trvCharacters.SelectedNode.Tag;

        statStatus.Text = "Added ImagePoint: " + cp.Add(e.X,e.Y,picCharacter.Size).ToString();
        CharacterPath_Updated(cp);
      }
    }

    private void CharacterPath_Updated(CharacterPath cp)
    {
      //picCharacter.Image = cp.Draw( true);
      statPointCount.Text = cp.ImagePoints.Count().ToString() + " Points";
      picCharacter.Image = (new AutoEdge(cp.Draw())).Image;
    }

    private void trvCharacters_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (trvCharacters.SelectedNode != null && trvCharacters.SelectedNode.Tag != null)
      {
        CharacterPath cp = (CharacterPath)trvCharacters.SelectedNode.Tag;
        CharacterPath_Updated(cp);
      }
    }

  }
}
