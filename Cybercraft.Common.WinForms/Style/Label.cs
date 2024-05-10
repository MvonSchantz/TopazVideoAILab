using System.Drawing;

namespace Cybercraft.Common.WinForms.Style
{
    public class Label : System.Windows.Forms.Label
    {
        public override Color BackColor { get; set; } = Style.Background;
        public override Color ForeColor { get; set; } = Style.Foreground;
    }
}
