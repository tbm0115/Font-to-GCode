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
  
  public partial class Settings_Characters : Form
  {
    public Settings_Characters()
    {
      InitializeComponent();

      this.DialogResult = DialogResult.Cancel;
      lstCharacters.Items.Clear();
      foreach (string item in Properties.Settings.Default.CharacterList)
      {
        lstCharacters.Items.Add(item);
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(txtNewCharacter.Text))
        return;
      lstCharacters.Items.Add(txtNewCharacter.Text);
      txtNewCharacter.Clear();
      txtNewCharacter.Focus();
      lstCharacters.SelectedIndex = lstCharacters.Items.Count - 1;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (lstCharacters.SelectedIndex < 0)
        return;
      lstCharacters.Items.RemoveAt(lstCharacters.SelectedIndex);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.CharacterList.Clear();
      foreach (string item in lstCharacters.Items)
      {
        Properties.Settings.Default.CharacterList.Add(item);
      }
      //// Remove items that were removed from list
      //for (int j = Properties.Settings.Default.CharacterList.Count; j >= 0 ; j--)
      //{
      //  bool found = false;
      //  for (int i = 0; i < lstCharacters.Items.Count; i++)
      //  {
      //    if (lstCharacters.Items[i].ToString() == Properties.Settings.Default.CharacterList[i])
      //    {
      //      found = true;
      //      break;
      //    }
      //  }
      //  if (!found)
      //  {
      //    Properties.Settings.Default.CharacterList.RemoveAt(j);
      //  }
      //}
      //// Add any new items
      //foreach (string item in lstCharacters.Items)
      //{
      //  if (!Properties.Settings.Default.CharacterList.Contains(item))
      //    Properties.Settings.Default.CharacterList.Add(item);
      //}
      Properties.Settings.Default.Save();
      this.DialogResult = DialogResult.OK;
    }
  }
}
