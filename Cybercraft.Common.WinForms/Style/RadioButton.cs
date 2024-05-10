using System.Drawing;

namespace Cybercraft.Common.WinForms.Style
{
    public class RadioButton : System.Windows.Forms.RadioButton
    {
        public override bool AutoSize { get; set; } = true;
        public override Color BackColor { get; set; } = Style.Background;
        public override Color ForeColor { get; set; } = Style.Foreground;
    }
}
