using System;
using System.Drawing;
using System.Windows.Forms;


namespace Cybercraft.Common.WinForms.Style
{
    public class DropDownList : System.Windows.Forms.ComboBox
    {
        private const int TextPadding = 10;

        private Bitmap _buffer;

        public DropDownList() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            DrawMode = DrawMode.OwnerDrawVariable;

            base.FlatStyle = FlatStyle.Flat;
            base.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _buffer = null;

            base.Dispose(disposing);
        }

        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);
            Invalidate();
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);
            Invalidate();
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
            Invalidate();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            PaintCombobox();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _buffer = null;
            Invalidate();
        }

        private void PaintCombobox()
        {
            if (_buffer == null)
                _buffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            using (var g = Graphics.FromImage(_buffer))
            {
                var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

                var textColor = Style.Foreground;
                var borderColor = Style.DarkBorder;
                var fillColor = Style.Background;

                if (Focused && TabStop)
                    borderColor = Style.Focus;

                using (var b = new SolidBrush(fillColor))
                {
                    g.FillRectangle(b, rect);
                }

                using (var p = new Pen(borderColor, 1))
                {
                    var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                    g.DrawRectangle(p, modRect);
                }

                var icon = Resources.scrollbar_arrow_hot;
                g.DrawImage(icon,
                                    rect.Right - icon.Width - (TextPadding / 2),
                                    (rect.Height / 2) - (icon.Height / 2));

                var text = SelectedItem != null ? SelectedItem.ToString() : Text;

                using (var b = new SolidBrush(textColor))
                {
                    var padding = 2;

                    var modRect = new Rectangle(rect.Left + padding,
                                                rect.Top + padding,
                                                rect.Width - icon.Width - (TextPadding / 2) - (padding * 2),
                                                rect.Height - (padding * 2));

                    var stringFormat = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    g.DrawString(text, Font, b, modRect, stringFormat);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_buffer == null)
                PaintCombobox();

            var g = e.Graphics;
            g.DrawImage(_buffer, Point.Empty);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var g = e.Graphics;
            var rect = e.Bounds;

            var textColor = Style.Foreground;
            var fillColor = Style.DarkBackground;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                (e.State & DrawItemState.Focus) == DrawItemState.Focus ||
                (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
                fillColor = Style.Focus;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                var text = Items[e.Index].ToString();

                using (var b = new SolidBrush(textColor))
                {
                    var padding = 2;

                    var modRect = new Rectangle(rect.Left + padding,
                        rect.Top + padding,
                        rect.Width - (padding * 2),
                        rect.Height - (padding * 2));

                    var stringFormat = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    g.DrawString(text, Font, b, modRect, stringFormat);
                }
            }
        }

    }
}
