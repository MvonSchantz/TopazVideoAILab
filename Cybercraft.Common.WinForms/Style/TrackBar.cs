using System.Drawing;
using System.Windows.Forms;

namespace Cybercraft.Common.WinForms.Style
{
    public class TrackBar : System.Windows.Forms.TrackBar
    {
        public override Color BackColor { get; set; } = Style.Background;
        public override Color ForeColor { get; set; } = Style.Foreground;

        public TrackBar()
        {
            TickStyle = TickStyle.Both;
        }
    }
}
