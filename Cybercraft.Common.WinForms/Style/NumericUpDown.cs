using System.Drawing;

namespace Cybercraft.Common.WinForms.Style
{
    public class NumericUpDown : System.Windows.Forms.NumericUpDown
    {
        public NumericUpDown()
        {
            BackColor = Style.Background;
            ForeColor = Style.Foreground;
        }
    }
}
