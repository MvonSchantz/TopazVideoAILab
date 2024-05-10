using System.Drawing;
using System.Windows.Forms;

namespace Cybercraft.Common.WinForms.Style
{
    public class Panel : System.Windows.Forms.Panel
    {
        public override Color BackColor { get; set; } = Style.Background;
        public override Color ForeColor { get; set; } = Style.Foreground;

        public Panel()
        {
            Dock = DockStyle.Fill;
        }
    }
}
