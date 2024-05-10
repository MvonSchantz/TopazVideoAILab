using System.Drawing;

namespace Cybercraft.Common.WinForms.Style
{
    public class TextBox : System.Windows.Forms.TextBox
    {

        public TextBox()
        {
            BackColor = Style.Background;
            ForeColor = Style.Foreground;
        }
    }
}
