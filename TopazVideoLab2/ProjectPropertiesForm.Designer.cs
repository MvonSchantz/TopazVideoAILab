
namespace TopazVideoLab2
{
    partial class ProjectPropertiesForm
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
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label ColorSpaceLabel;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label InterlaceLabel;
            this.ColorSpaceDropDownList = new System.Windows.Forms.ComboBox();
            this.ResolutionComboBox = new System.Windows.Forms.ComboBox();
            this.OutputHeightTextBox = new System.Windows.Forms.TextBox();
            this.OutputWidthTextBox = new System.Windows.Forms.TextBox();
            this.InputHeightTextBox = new System.Windows.Forms.TextBox();
            this.InputWidthTextBox = new System.Windows.Forms.TextBox();
            this.AspectRatioDropdownList = new System.Windows.Forms.ComboBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButtonControl = new System.Windows.Forms.Button();
            this.DeinterlaceDropDownList = new System.Windows.Forms.ComboBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            ColorSpaceLabel = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            InterlaceLabel = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.DeinterlaceDropDownList);
            groupBox1.Controls.Add(InterlaceLabel);
            groupBox1.Controls.Add(this.ColorSpaceDropDownList);
            groupBox1.Controls.Add(ColorSpaceLabel);
            groupBox1.Controls.Add(this.ResolutionComboBox);
            groupBox1.Controls.Add(this.OutputHeightTextBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(this.OutputWidthTextBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(this.InputHeightTextBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(this.InputWidthTextBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(this.AspectRatioDropdownList);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(410, 177);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Size";
            // 
            // ColorSpaceDropDownList
            // 
            this.ColorSpaceDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorSpaceDropDownList.FormattingEnabled = true;
            this.ColorSpaceDropDownList.Items.AddRange(new object[] {
            "Rec.601",
            "Rec.709"});
            this.ColorSpaceDropDownList.Location = new System.Drawing.Point(85, 56);
            this.ColorSpaceDropDownList.Name = "ColorSpaceDropDownList";
            this.ColorSpaceDropDownList.Size = new System.Drawing.Size(100, 23);
            this.ColorSpaceDropDownList.TabIndex = 3;
            // 
            // ColorSpaceLabel
            // 
            ColorSpaceLabel.AutoSize = true;
            ColorSpaceLabel.Location = new System.Drawing.Point(6, 59);
            ColorSpaceLabel.Name = "ColorSpaceLabel";
            ColorSpaceLabel.Size = new System.Drawing.Size(72, 15);
            ColorSpaceLabel.TabIndex = 2;
            ColorSpaceLabel.Text = "Color space:";
            // 
            // ResolutionComboBox
            // 
            this.ResolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResolutionComboBox.FormattingEnabled = true;
            this.ResolutionComboBox.Items.AddRange(new object[] {
            "Custom",
            "720p",
            "1080p",
            "1440p",
            "2160p"});
            this.ResolutionComboBox.Location = new System.Drawing.Point(316, 143);
            this.ResolutionComboBox.Name = "ResolutionComboBox";
            this.ResolutionComboBox.Size = new System.Drawing.Size(84, 23);
            this.ResolutionComboBox.TabIndex = 12;
            this.ResolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.ResolutionComboBox_SelectedIndexChanged);
            // 
            // OutputHeightTextBox
            // 
            this.OutputHeightTextBox.Location = new System.Drawing.Point(210, 143);
            this.OutputHeightTextBox.Name = "OutputHeightTextBox";
            this.OutputHeightTextBox.Size = new System.Drawing.Size(100, 23);
            this.OutputHeightTextBox.TabIndex = 11;
            this.OutputHeightTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OutputTextBox_Validating);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(191, 146);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(13, 15);
            label5.TabIndex = 10;
            label5.Text = "x";
            // 
            // OutputWidthTextBox
            // 
            this.OutputWidthTextBox.Location = new System.Drawing.Point(85, 143);
            this.OutputWidthTextBox.Name = "OutputWidthTextBox";
            this.OutputWidthTextBox.Size = new System.Drawing.Size(100, 23);
            this.OutputWidthTextBox.TabIndex = 9;
            this.OutputWidthTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OutputTextBox_Validating);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 146);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(70, 15);
            label4.TabIndex = 8;
            label4.Text = "Output size:";
            // 
            // InputHeightTextBox
            // 
            this.InputHeightTextBox.Location = new System.Drawing.Point(210, 114);
            this.InputHeightTextBox.Name = "InputHeightTextBox";
            this.InputHeightTextBox.ReadOnly = true;
            this.InputHeightTextBox.Size = new System.Drawing.Size(100, 23);
            this.InputHeightTextBox.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(191, 117);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(13, 15);
            label3.TabIndex = 6;
            label3.Text = "x";
            // 
            // InputWidthTextBox
            // 
            this.InputWidthTextBox.Location = new System.Drawing.Point(85, 114);
            this.InputWidthTextBox.Name = "InputWidthTextBox";
            this.InputWidthTextBox.ReadOnly = true;
            this.InputWidthTextBox.Size = new System.Drawing.Size(100, 23);
            this.InputWidthTextBox.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 117);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 15);
            label2.TabIndex = 4;
            label2.Text = "Input size:";
            // 
            // AspectRatioDropdownList
            // 
            this.AspectRatioDropdownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AspectRatioDropdownList.FormattingEnabled = true;
            this.AspectRatioDropdownList.Items.AddRange(new object[] {
            "4:3",
            "16:9"});
            this.AspectRatioDropdownList.Location = new System.Drawing.Point(85, 25);
            this.AspectRatioDropdownList.Name = "AspectRatioDropdownList";
            this.AspectRatioDropdownList.Size = new System.Drawing.Size(100, 23);
            this.AspectRatioDropdownList.TabIndex = 1;
            this.AspectRatioDropdownList.SelectedIndexChanged += new System.EventHandler(this.AspectRatioDropdownList_SelectedIndexChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 28);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(73, 15);
            label1.TabIndex = 0;
            label1.Text = "Aspect ratio:";
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(347, 195);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "&Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButtonControl
            // 
            this.CancelButtonControl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButtonControl.Location = new System.Drawing.Point(266, 195);
            this.CancelButtonControl.Name = "CancelButtonControl";
            this.CancelButtonControl.Size = new System.Drawing.Size(75, 23);
            this.CancelButtonControl.TabIndex = 1;
            this.CancelButtonControl.Text = "&Cancel";
            this.CancelButtonControl.UseVisualStyleBackColor = true;
            // 
            // DeinterlaceDropDownList
            // 
            this.DeinterlaceDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeinterlaceDropDownList.FormattingEnabled = true;
            this.DeinterlaceDropDownList.Items.AddRange(new object[] {
            "None",
            "IVTC"});
            this.DeinterlaceDropDownList.Location = new System.Drawing.Point(85, 85);
            this.DeinterlaceDropDownList.Name = "DeinterlaceDropDownList";
            this.DeinterlaceDropDownList.Size = new System.Drawing.Size(100, 23);
            this.DeinterlaceDropDownList.TabIndex = 14;
            // 
            // InterlaceLabel
            // 
            InterlaceLabel.AutoSize = true;
            InterlaceLabel.Location = new System.Drawing.Point(6, 88);
            InterlaceLabel.Name = "InterlaceLabel";
            InterlaceLabel.Size = new System.Drawing.Size(69, 15);
            InterlaceLabel.TabIndex = 13;
            InterlaceLabel.Text = "Deinterlace:";
            // 
            // ProjectPropertiesForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButtonControl;
            this.ClientSize = new System.Drawing.Size(432, 226);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.CancelButtonControl);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectPropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project properties";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button CancelButtonControl;
        internal System.Windows.Forms.TextBox InputHeightTextBox;
        internal System.Windows.Forms.TextBox InputWidthTextBox;
        internal System.Windows.Forms.ComboBox AspectRatioDropdownList;
        internal System.Windows.Forms.TextBox OutputHeightTextBox;
        internal System.Windows.Forms.TextBox OutputWidthTextBox;
        private ComboBox ResolutionComboBox;
        internal ComboBox ColorSpaceDropDownList;
        internal ComboBox DeinterlaceDropDownList;
    }
}