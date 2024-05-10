using System.Drawing;

namespace Cybercraft.Common.WinForms.Style
{
    public class CheckBox : System.Windows.Forms.CheckBox
    {
        public override bool AutoSize { get; set; } = true;
        public override Color BackColor { get; set; } = Style.Background;
        public override Color ForeColor { get; set; } = Style.Foreground;
    }
}
