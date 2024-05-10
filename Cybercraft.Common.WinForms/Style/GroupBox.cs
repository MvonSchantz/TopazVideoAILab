using System.Drawing;

namespace Cybercraft.Common.WinForms.Style
{
    public class GroupBox : System.Windows.Forms.GroupBox
    {
        //public override Color BackColor { get; set; } = Style.Background;
        //public override Color ForeColor { get; set; } = Style.Foreground;

        private const int TextPadding = 10;

        public GroupBox()
        {
            ForeColor = Style.Foreground;
            BackColor = Style.Background;

            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer |
                     System.Windows.Forms.ControlStyles.ResizeRedraw |
                     System.Windows.Forms.ControlStyles.UserPaint, true);

            ResizeRedraw = true;
            DoubleBuffered = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            var stringSize = g.MeasureString(Text, Font);

            var textColor = Style.Foreground;
            var fillColor = Style.Background;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            using (var p = new Pen(Style.DarkBorder, 1))
            {
                var borderRect = new Rectangle(0, (int)stringSize.Height / 2, rect.Width - 1, rect.Height - ((int)stringSize.Height / 2) - 1);
                g.DrawRectangle(p, borderRect);
            }

            var textRect = new Rectangle(rect.Left + TextPadding,
                rect.Top,
                rect.Width - (TextPadding * 2),
                (int)stringSize.Height);

            using (var b2 = new SolidBrush(fillColor))
            {
                var modRect = new Rectangle(textRect.Left, textRect.Top, System.Math.Min(textRect.Width, (int)stringSize.Width), textRect.Height);
                g.FillRectangle(b2, modRect);
            }

            using (var b = new SolidBrush(textColor))
            {
                var stringFormat = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    FormatFlags = StringFormatFlags.NoWrap,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(Text, Font, b, textRect, stringFormat);
            }
        }

    }
}
