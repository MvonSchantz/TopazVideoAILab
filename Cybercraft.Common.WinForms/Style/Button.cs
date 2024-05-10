using System.Drawing;
using System.Windows.Forms;

namespace Cybercraft.Common.WinForms.Style
{
    public class Button : System.Windows.Forms.Button
    {
        private enum State
        {
            Normal,
            Hover,
            Pressed,
        }

        private State ButtonState { get; set; }

        private bool SpacePressed { get; set; }

        private bool IsAccept { get; set; }

        public int ImagePadding { get; set; } = 5;

        public Button()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            base.UseVisualStyleBackColor = false;
            base.UseCompatibleTextRendering = false;

            SetButtonState(State.Normal);
           // Padding = new Padding(_padding);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            var form = FindForm();
            if (form != null)
            {
                if (form.AcceptButton == this)
                    IsAccept = true;
            }
        }

        private void SetButtonState(State buttonState)
        {
            if (ButtonState != buttonState)
            {
                ButtonState = buttonState;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (SpacePressed)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (ClientRectangle.Contains(e.Location))
                    SetButtonState(State.Pressed);
                else
                    SetButtonState(State.Hover);
            }
            else
            {
                SetButtonState(State.Hover);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!ClientRectangle.Contains(e.Location))
                return;

            SetButtonState(State.Pressed);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (SpacePressed)
                return;

            SetButtonState(State.Normal);
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);

            if (SpacePressed)
                return;

            SetButtonState(State.Normal);
        }

        protected override void OnMouseCaptureChanged(System.EventArgs e)
        {
            base.OnMouseCaptureChanged(e);

            if (SpacePressed)
                return;

            var location = Cursor.Position;

            if (!ClientRectangle.Contains(location))
                SetButtonState(State.Normal);
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);

            Invalidate();
        }

        protected override void OnLostFocus(System.EventArgs e)
        {
            base.OnLostFocus(e);

            SpacePressed = false;

            var location = Cursor.Position;

            if (!ClientRectangle.Contains(location))
                SetButtonState(State.Normal);
            else
                SetButtonState(State.Hover);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space)
            {
                SpacePressed = true;
                SetButtonState(State.Pressed);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Space)
            {
                SpacePressed = false;

                var location = Cursor.Position;

                if (!ClientRectangle.Contains(location))
                    SetButtonState(State.Normal);
                else
                    SetButtonState(State.Hover);
            }
        }

        public override void NotifyDefault(bool value)
        {
            base.NotifyDefault(value);

            if (!DesignMode)
                return;

            IsAccept = value;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            var textColor = Style.Foreground;
            var borderColor = Style.Border;
            var fillColor = IsAccept ? Style.Border : Style.Background; //_isDefault ? Colors.DarkBlueBackground : Colors.LightBackground;

            if (Enabled)
            {
                if (true)
                {
                    if (Focused && TabStop)
                        borderColor = Style.Focus; //Colors.BlueHighlight;

                    switch (ButtonState)
                    {
                        case State.Hover:
                            fillColor = Style.Border; //_isDefault ? Colors.BlueBackground : Colors.LighterBackground;
                            break;
                        case State.Pressed:
                            fillColor = Style.Focus; //_isDefault ? Colors.DarkBackground : Colors.DarkBackground;
                            textColor = Style.DarkBackground;
                            break;
                    }
                }
                
            }
            else
            {
                textColor = Style.Border;//Colors.DisabledText;
                fillColor = Style.Background;//Colors.DarkGreySelection;
            }

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            using (var p = new Pen(borderColor, 1))
            {
                var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);

                g.DrawRectangle(p, modRect);
            }

            var textOffsetX = 0;
            var textOffsetY = 0;

            if (Image != null)
            {
                var stringSize = g.MeasureString(Text, Font, rect.Size);

                var x = (ClientSize.Width / 2) - (Image.Size.Width / 2);
                var y = (ClientSize.Height / 2) - (Image.Size.Height / 2);

                switch (TextImageRelation)
                {
                    case TextImageRelation.ImageAboveText:
                        textOffsetY = (Image.Size.Height / 2) + (ImagePadding / 2);
                        y = y - ((int)(stringSize.Height / 2) + (ImagePadding / 2));
                        break;
                    case TextImageRelation.TextAboveImage:
                        textOffsetY = ((Image.Size.Height / 2) + (ImagePadding / 2)) * -1;
                        y = y + ((int)(stringSize.Height / 2) + (ImagePadding / 2));
                        break;
                    case TextImageRelation.ImageBeforeText:
                        textOffsetX = Image.Size.Width + (ImagePadding * 2);
                        x = ImagePadding;
                        break;
                    case TextImageRelation.TextBeforeImage:
                        x = x + (int)stringSize.Width;
                        break;
                }

                g.DrawImage(Image, x, y);
            }

            using (var b = new SolidBrush(textColor))
            {
                var modRect = new Rectangle(rect.Left + textOffsetX + Padding.Left,
                                            rect.Top + textOffsetY + Padding.Top, rect.Width - Padding.Horizontal,
                                            rect.Height - Padding.Vertical);

                var stringFormat = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(Text, Font, b, modRect, stringFormat);
            }
        }
    }
}
