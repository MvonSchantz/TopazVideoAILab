
namespace TopazVideoLab2
{
    partial class PreviewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PaintPanel = new Cybercraft.Common.WinForms.PaintPanel();
            this.SuspendLayout();
            // 
            // PaintPanel
            // 
            this.PaintPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaintPanel.Location = new System.Drawing.Point(0, 0);
            this.PaintPanel.Name = "PaintPanel";
            this.PaintPanel.Size = new System.Drawing.Size(992, 718);
            this.PaintPanel.TabIndex = 0;
            this.PaintPanel.MouseWheelScroll += new Cybercraft.Common.WinForms.PaintPanel.MouseWheelEventHandler(this.PaintPanel_MouseWheelScroll);
            this.PaintPanel.PaintDoubleBuffer += new System.Windows.Forms.PaintEventHandler(this.PaintPanel_PaintDoubleBuffer);
            this.PaintPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PaintPanel_MouseDown);
            this.PaintPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaintPanel_MouseMove);
            this.PaintPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaintPanel_MouseUp);
            this.PaintPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PaintPanel_PreviewKeyDown);
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 718);
            this.ControlBox = false;
            this.Controls.Add(this.PaintPanel);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "PreviewForm";
            this.ShowInTaskbar = false;
            this.Text = "Preview - Use keys 1-9 to change node, mouse wheel to zoom,  mouse left button to" +
    " drag";
            this.ResumeLayout(false);

        }

        #endregion

        private Cybercraft.Common.WinForms.PaintPanel PaintPanel;
    }
}