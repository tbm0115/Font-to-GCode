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
  public partial class Settings_GCode : Form
  {
    public Settings_GCode()
    {
      InitializeComponent();

      this.DialogResult = DialogResult.Cancel;
    }

    private void textbox_KeyPress(object sender, KeyPressEventArgs e)
    {
      TextBox txt = (TextBox)sender;
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

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }
  }
}
