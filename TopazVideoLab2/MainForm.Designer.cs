namespace TopazVideoLab2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ToolStrip = new ToolStrip();
            MenuStrip = new MenuStrip();
            FileMenuItem = new ToolStripMenuItem();
            NewMenuItem = new ToolStripMenuItem();
            OpenMenuItem = new ToolStripMenuItem();
            SaveMenuItem = new ToolStripMenuItem();
            SaveAsMenuItem = new ToolStripMenuItem();
            Break1MenuItem = new ToolStripSeparator();
            ExitMenuItem = new ToolStripMenuItem();
            StatusStrip = new StatusStrip();
            TableLayoutPanel = new TableLayoutPanel();
            PreviewControlPanel = new Panel();
            PreviewScrubTrackBar = new TrackBar();
            RenderLengthComboBox = new ComboBox();
            RenderButton = new Button();
            FullScrubTrackBar = new TrackBar();
            GraphPanel = new Panel();
            GraphPaintPanel = new Cybercraft.Common.WinForms.PaintPanel();
            StepPropertiesPanel = new Panel();
            NoisePresetDropDownList = new ComboBox();
            RecoverOriginalDetailsUpDown = new NumericUpDown();
            RecoverOriginalDetailsLabel = new Label();
            OffsetYUpDown = new NumericUpDown();
            OffsetXUpDown = new NumericUpDown();
            OffsetLabel = new Label();
            AntiAliasDeblurUpDown = new NumericUpDown();
            AntiAliasDeblurLabel = new Label();
            DehaloUpDown = new NumericUpDown();
            DehaloLabel = new Label();
            ReduceNoiseUpDown = new NumericUpDown();
            ReduceNoiseLabel = new Label();
            SharpenUpDown = new NumericUpDown();
            SharpenLabel = new Label();
            RecoverDetailsUpDown = new NumericUpDown();
            RecoverDetailsLabel = new Label();
            RevertCompressionUpDown = new NumericUpDown();
            RevertCompressionLabel = new Label();
            AutoCheckBox = new CheckBox();
            WeightLabel = new Label();
            WeightUpDown = new NumericUpDown();
            HeightUpDown = new NumericUpDown();
            WidthUpDown = new NumericUpDown();
            OutputResolutionTextBox = new TextBox();
            UpscaleFactorDropDownList = new ComboBox();
            UpscaleAlgorithmDropDownList = new ComboBox();
            UpscaleLabel = new Label();
            NoiseLabel = new Label();
            DownscaleAlgorithmDropDownList = new ComboBox();
            ResizeLabel = new Label();
            PresetDropDownList = new ComboBox();
            NoiseUpDown = new NumericUpDown();
            OpenProjectDialog = new OpenFileDialog();
            SaveProjectDialog = new SaveFileDialog();
            OpenVideoDialog = new OpenFileDialog();
            MenuStrip.SuspendLayout();
            TableLayoutPanel.SuspendLayout();
            PreviewControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewScrubTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FullScrubTrackBar).BeginInit();
            GraphPanel.SuspendLayout();
            StepPropertiesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RecoverOriginalDetailsUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)OffsetYUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)OffsetXUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AntiAliasDeblurUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DehaloUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ReduceNoiseUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SharpenUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RecoverDetailsUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RevertCompressionUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)WeightUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HeightUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)WidthUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NoiseUpDown).BeginInit();
            SuspendLayout();
            // 
            // ToolStrip
            // 
            ToolStrip.Location = new Point(0, 24);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.Size = new Size(1231, 25);
            ToolStrip.TabIndex = 1;
            ToolStrip.Text = "toolStrip1";
            // 
            // MenuStrip
            // 
            MenuStrip.Items.AddRange(new ToolStripItem[] { FileMenuItem });
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new Size(1231, 24);
            MenuStrip.TabIndex = 0;
            MenuStrip.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            FileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { NewMenuItem, OpenMenuItem, SaveMenuItem, SaveAsMenuItem, Break1MenuItem, ExitMenuItem });
            FileMenuItem.Name = "FileMenuItem";
            FileMenuItem.Size = new Size(37, 20);
            FileMenuItem.Text = "&File";
            // 
            // NewMenuItem
            // 
            NewMenuItem.Name = "NewMenuItem";
            NewMenuItem.Size = new Size(121, 22);
            NewMenuItem.Text = "&New...";
            NewMenuItem.Click += NewMenuItem_Click;
            // 
            // OpenMenuItem
            // 
            OpenMenuItem.Name = "OpenMenuItem";
            OpenMenuItem.Size = new Size(121, 22);
            OpenMenuItem.Text = "&Open...";
            OpenMenuItem.Click += OpenMenuItem_Click;
            // 
            // SaveMenuItem
            // 
            SaveMenuItem.Name = "SaveMenuItem";
            SaveMenuItem.Size = new Size(121, 22);
            SaveMenuItem.Text = "&Save";
            SaveMenuItem.Click += SaveMenuItem_Click;
            // 
            // SaveAsMenuItem
            // 
            SaveAsMenuItem.Name = "SaveAsMenuItem";
            SaveAsMenuItem.Size = new Size(121, 22);
            SaveAsMenuItem.Text = "Save &as...";
            SaveAsMenuItem.Click += SaveAsMenuItem_Click;
            // 
            // Break1MenuItem
            // 
            Break1MenuItem.Name = "Break1MenuItem";
            Break1MenuItem.Size = new Size(118, 6);
            // 
            // ExitMenuItem
            // 
            ExitMenuItem.Name = "ExitMenuItem";
            ExitMenuItem.Size = new Size(121, 22);
            ExitMenuItem.Text = "E&xit";
            ExitMenuItem.Click += ExitMenuItem_Click;
            // 
            // StatusStrip
            // 
            StatusStrip.Location = new Point(0, 927);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new Size(1231, 22);
            StatusStrip.TabIndex = 2;
            StatusStrip.Text = "statusStrip1";
            // 
            // TableLayoutPanel
            // 
            TableLayoutPanel.ColumnCount = 2;
            TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350F));
            TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TableLayoutPanel.Controls.Add(PreviewControlPanel, 0, 1);
            TableLayoutPanel.Controls.Add(GraphPanel, 1, 0);
            TableLayoutPanel.Controls.Add(StepPropertiesPanel, 0, 0);
            TableLayoutPanel.Dock = DockStyle.Fill;
            TableLayoutPanel.Location = new Point(0, 49);
            TableLayoutPanel.Name = "TableLayoutPanel";
            TableLayoutPanel.RowCount = 2;
            TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            TableLayoutPanel.Size = new Size(1231, 878);
            TableLayoutPanel.TabIndex = 6;
            // 
            // PreviewControlPanel
            // 
            PreviewControlPanel.BorderStyle = BorderStyle.Fixed3D;
            PreviewControlPanel.Controls.Add(PreviewScrubTrackBar);
            PreviewControlPanel.Controls.Add(RenderLengthComboBox);
            PreviewControlPanel.Controls.Add(RenderButton);
            PreviewControlPanel.Controls.Add(FullScrubTrackBar);
            PreviewControlPanel.Dock = DockStyle.Fill;
            PreviewControlPanel.Location = new Point(3, 659);
            PreviewControlPanel.Margin = new Padding(3, 1, 1, 3);
            PreviewControlPanel.Name = "PreviewControlPanel";
            PreviewControlPanel.Size = new Size(346, 216);
            PreviewControlPanel.TabIndex = 1;
            // 
            // PreviewScrubTrackBar
            // 
            PreviewScrubTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PreviewScrubTrackBar.Location = new Point(7, 112);
            PreviewScrubTrackBar.Maximum = 1;
            PreviewScrubTrackBar.Name = "PreviewScrubTrackBar";
            PreviewScrubTrackBar.Size = new Size(332, 45);
            PreviewScrubTrackBar.TabIndex = 5;
            PreviewScrubTrackBar.Scroll += PreviewScrubTrackBar_Scroll;
            // 
            // RenderLengthComboBox
            // 
            RenderLengthComboBox.FormattingEnabled = true;
            RenderLengthComboBox.Items.AddRange(new object[] { "All", "4 frames", "10 frames", "1 second", "5 seconds", "30 seconds" });
            RenderLengthComboBox.Location = new Point(7, 32);
            RenderLengthComboBox.Name = "RenderLengthComboBox";
            RenderLengthComboBox.Size = new Size(121, 23);
            RenderLengthComboBox.TabIndex = 1;
            RenderLengthComboBox.SelectedIndexChanged += RenderLengthComboBox_SelectedIndexChanged;
            RenderLengthComboBox.TextChanged += RenderLengthComboBox_TextChanged;
            // 
            // RenderButton
            // 
            RenderButton.Location = new Point(7, 3);
            RenderButton.Name = "RenderButton";
            RenderButton.Size = new Size(75, 23);
            RenderButton.TabIndex = 0;
            RenderButton.Text = "Render";
            RenderButton.UseVisualStyleBackColor = true;
            RenderButton.Click += RenderButton_Click;
            // 
            // FullScrubTrackBar
            // 
            FullScrubTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FullScrubTrackBar.Location = new Point(7, 61);
            FullScrubTrackBar.Maximum = 1;
            FullScrubTrackBar.Name = "FullScrubTrackBar";
            FullScrubTrackBar.Size = new Size(332, 45);
            FullScrubTrackBar.TabIndex = 2;
            FullScrubTrackBar.Scroll += FullScrubTrackBar_Scroll;
            // 
            // GraphPanel
            // 
            GraphPanel.BorderStyle = BorderStyle.Fixed3D;
            GraphPanel.Controls.Add(GraphPaintPanel);
            GraphPanel.Dock = DockStyle.Fill;
            GraphPanel.Location = new Point(351, 3);
            GraphPanel.Margin = new Padding(1, 3, 3, 3);
            GraphPanel.Name = "GraphPanel";
            TableLayoutPanel.SetRowSpan(GraphPanel, 2);
            GraphPanel.Size = new Size(877, 872);
            GraphPanel.TabIndex = 0;
            // 
            // GraphPaintPanel
            // 
            GraphPaintPanel.AntiAliasLevel = 1;
            GraphPaintPanel.Dock = DockStyle.Fill;
            GraphPaintPanel.Location = new Point(0, 0);
            GraphPaintPanel.Name = "GraphPaintPanel";
            GraphPaintPanel.Size = new Size(873, 868);
            GraphPaintPanel.TabIndex = 0;
            GraphPaintPanel.PaintGdi += GraphPaintPanel_PaintGdi;
            GraphPaintPanel.SizeChanged += GraphPaintPanel_SizeChanged;
            GraphPaintPanel.MouseDoubleClick += GraphPaintPanel_MouseDoubleClick;
            GraphPaintPanel.MouseDown += GraphPaintPanel_MouseDown;
            GraphPaintPanel.MouseMove += GraphPaintPanel_MouseMove;
            GraphPaintPanel.MouseUp += GraphPaintPanel_MouseUp;
            GraphPaintPanel.PreviewKeyDown += GraphPaintPanel_PreviewKeyDown;
            // 
            // StepPropertiesPanel
            // 
            StepPropertiesPanel.BorderStyle = BorderStyle.Fixed3D;
            StepPropertiesPanel.Controls.Add(NoisePresetDropDownList);
            StepPropertiesPanel.Controls.Add(RecoverOriginalDetailsUpDown);
            StepPropertiesPanel.Controls.Add(RecoverOriginalDetailsLabel);
            StepPropertiesPanel.Controls.Add(OffsetYUpDown);
            StepPropertiesPanel.Controls.Add(OffsetXUpDown);
            StepPropertiesPanel.Controls.Add(OffsetLabel);
            StepPropertiesPanel.Controls.Add(AntiAliasDeblurUpDown);
            StepPropertiesPanel.Controls.Add(AntiAliasDeblurLabel);
            StepPropertiesPanel.Controls.Add(DehaloUpDown);
            StepPropertiesPanel.Controls.Add(DehaloLabel);
            StepPropertiesPanel.Controls.Add(ReduceNoiseUpDown);
            StepPropertiesPanel.Controls.Add(ReduceNoiseLabel);
            StepPropertiesPanel.Controls.Add(SharpenUpDown);
            StepPropertiesPanel.Controls.Add(SharpenLabel);
            StepPropertiesPanel.Controls.Add(RecoverDetailsUpDown);
            StepPropertiesPanel.Controls.Add(RecoverDetailsLabel);
            StepPropertiesPanel.Controls.Add(RevertCompressionUpDown);
            StepPropertiesPanel.Controls.Add(RevertCompressionLabel);
            StepPropertiesPanel.Controls.Add(AutoCheckBox);
            StepPropertiesPanel.Controls.Add(WeightLabel);
            StepPropertiesPanel.Controls.Add(WeightUpDown);
            StepPropertiesPanel.Controls.Add(HeightUpDown);
            StepPropertiesPanel.Controls.Add(WidthUpDown);
            StepPropertiesPanel.Controls.Add(OutputResolutionTextBox);
            StepPropertiesPanel.Controls.Add(UpscaleFactorDropDownList);
            StepPropertiesPanel.Controls.Add(UpscaleAlgorithmDropDownList);
            StepPropertiesPanel.Controls.Add(UpscaleLabel);
            StepPropertiesPanel.Controls.Add(NoiseLabel);
            StepPropertiesPanel.Controls.Add(DownscaleAlgorithmDropDownList);
            StepPropertiesPanel.Controls.Add(ResizeLabel);
            StepPropertiesPanel.Controls.Add(PresetDropDownList);
            StepPropertiesPanel.Controls.Add(NoiseUpDown);
            StepPropertiesPanel.Dock = DockStyle.Fill;
            StepPropertiesPanel.Location = new Point(3, 3);
            StepPropertiesPanel.Margin = new Padding(3, 3, 1, 1);
            StepPropertiesPanel.Name = "StepPropertiesPanel";
            StepPropertiesPanel.Size = new Size(346, 654);
            StepPropertiesPanel.TabIndex = 0;
            // 
            // NoisePresetDropDownList
            // 
            NoisePresetDropDownList.DropDownStyle = ComboBoxStyle.DropDownList;
            NoisePresetDropDownList.FormattingEnabled = true;
            NoisePresetDropDownList.Items.AddRange(new object[] { "QTGMC Placebo", "QTGMC Very Slow", "QTGMC Slower", "QTGMC Slow", "QTGMC Medium", "QTGMC Fast", "QTGMC Faster", "QTGMC Very Fast", "QTGMC Super Fast", "QTGMC Ultra Fast", "QTGMC Draft", "FilmGrain 0.05", "FilmGrain 0.10", "FilmGrain 0.15", "FilmGrain 0.20" });
            NoisePresetDropDownList.Location = new Point(170, 116);
            NoisePresetDropDownList.MaxDropDownItems = 3;
            NoisePresetDropDownList.Name = "NoisePresetDropDownList";
            NoisePresetDropDownList.Size = new Size(121, 23);
            NoisePresetDropDownList.TabIndex = 31;
            NoisePresetDropDownList.Visible = false;
            NoisePresetDropDownList.SelectedIndexChanged += NoisePresetDropDownList_SelectedIndexChanged;
            // 
            // RecoverOriginalDetailsUpDown
            // 
            RecoverOriginalDetailsUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            RecoverOriginalDetailsUpDown.Location = new Point(191, 461);
            RecoverOriginalDetailsUpDown.Name = "RecoverOriginalDetailsUpDown";
            RecoverOriginalDetailsUpDown.Size = new Size(100, 23);
            RecoverOriginalDetailsUpDown.TabIndex = 25;
            RecoverOriginalDetailsUpDown.Visible = false;
            RecoverOriginalDetailsUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // RecoverOriginalDetailsLabel
            // 
            RecoverOriginalDetailsLabel.AutoSize = true;
            RecoverOriginalDetailsLabel.Location = new Point(64, 463);
            RecoverOriginalDetailsLabel.Name = "RecoverOriginalDetailsLabel";
            RecoverOriginalDetailsLabel.Size = new Size(116, 15);
            RecoverOriginalDetailsLabel.TabIndex = 24;
            RecoverOriginalDetailsLabel.Text = "Recover orig. details:";
            RecoverOriginalDetailsLabel.Visible = false;
            // 
            // OffsetYUpDown
            // 
            OffsetYUpDown.DecimalPlaces = 2;
            OffsetYUpDown.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            OffsetYUpDown.Location = new Point(245, 506);
            OffsetYUpDown.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            OffsetYUpDown.Minimum = new decimal(new int[] { 4, 0, 0, int.MinValue });
            OffsetYUpDown.Name = "OffsetYUpDown";
            OffsetYUpDown.Size = new Size(46, 23);
            OffsetYUpDown.TabIndex = 28;
            OffsetYUpDown.Visible = false;
            OffsetYUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // OffsetXUpDown
            // 
            OffsetXUpDown.DecimalPlaces = 2;
            OffsetXUpDown.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            OffsetXUpDown.Location = new Point(191, 506);
            OffsetXUpDown.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            OffsetXUpDown.Minimum = new decimal(new int[] { 4, 0, 0, int.MinValue });
            OffsetXUpDown.Name = "OffsetXUpDown";
            OffsetXUpDown.Size = new Size(46, 23);
            OffsetXUpDown.TabIndex = 27;
            OffsetXUpDown.Visible = false;
            OffsetXUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // OffsetLabel
            // 
            OffsetLabel.AutoSize = true;
            OffsetLabel.Location = new Point(64, 508);
            OffsetLabel.Name = "OffsetLabel";
            OffsetLabel.Size = new Size(42, 15);
            OffsetLabel.TabIndex = 26;
            OffsetLabel.Text = "Offset:";
            OffsetLabel.Visible = false;
            // 
            // AntiAliasDeblurUpDown
            // 
            AntiAliasDeblurUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            AntiAliasDeblurUpDown.Location = new Point(191, 415);
            AntiAliasDeblurUpDown.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            AntiAliasDeblurUpDown.Name = "AntiAliasDeblurUpDown";
            AntiAliasDeblurUpDown.Size = new Size(100, 23);
            AntiAliasDeblurUpDown.TabIndex = 23;
            AntiAliasDeblurUpDown.Visible = false;
            AntiAliasDeblurUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // AntiAliasDeblurLabel
            // 
            AntiAliasDeblurLabel.AutoSize = true;
            AntiAliasDeblurLabel.Location = new Point(64, 417);
            AntiAliasDeblurLabel.Name = "AntiAliasDeblurLabel";
            AntiAliasDeblurLabel.Size = new Size(100, 15);
            AntiAliasDeblurLabel.TabIndex = 22;
            AntiAliasDeblurLabel.Text = "Anti-alias/Deblur:";
            AntiAliasDeblurLabel.Visible = false;
            // 
            // DehaloUpDown
            // 
            DehaloUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            DehaloUpDown.Location = new Point(191, 386);
            DehaloUpDown.Name = "DehaloUpDown";
            DehaloUpDown.Size = new Size(100, 23);
            DehaloUpDown.TabIndex = 21;
            DehaloUpDown.Visible = false;
            DehaloUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // DehaloLabel
            // 
            DehaloLabel.AutoSize = true;
            DehaloLabel.Location = new Point(64, 388);
            DehaloLabel.Name = "DehaloLabel";
            DehaloLabel.Size = new Size(47, 15);
            DehaloLabel.TabIndex = 20;
            DehaloLabel.Text = "Dehalo:";
            DehaloLabel.Visible = false;
            // 
            // ReduceNoiseUpDown
            // 
            ReduceNoiseUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            ReduceNoiseUpDown.Location = new Point(191, 357);
            ReduceNoiseUpDown.Name = "ReduceNoiseUpDown";
            ReduceNoiseUpDown.Size = new Size(100, 23);
            ReduceNoiseUpDown.TabIndex = 19;
            ReduceNoiseUpDown.Visible = false;
            ReduceNoiseUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // ReduceNoiseLabel
            // 
            ReduceNoiseLabel.AutoSize = true;
            ReduceNoiseLabel.Location = new Point(64, 359);
            ReduceNoiseLabel.Name = "ReduceNoiseLabel";
            ReduceNoiseLabel.Size = new Size(80, 15);
            ReduceNoiseLabel.TabIndex = 18;
            ReduceNoiseLabel.Text = "Reduce noise:";
            ReduceNoiseLabel.Visible = false;
            // 
            // SharpenUpDown
            // 
            SharpenUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            SharpenUpDown.Location = new Point(191, 328);
            SharpenUpDown.Name = "SharpenUpDown";
            SharpenUpDown.Size = new Size(100, 23);
            SharpenUpDown.TabIndex = 17;
            SharpenUpDown.Visible = false;
            SharpenUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // SharpenLabel
            // 
            SharpenLabel.AutoSize = true;
            SharpenLabel.Location = new Point(64, 330);
            SharpenLabel.Name = "SharpenLabel";
            SharpenLabel.Size = new Size(53, 15);
            SharpenLabel.TabIndex = 16;
            SharpenLabel.Text = "Sharpen:";
            SharpenLabel.Visible = false;
            // 
            // RecoverDetailsUpDown
            // 
            RecoverDetailsUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            RecoverDetailsUpDown.Location = new Point(191, 299);
            RecoverDetailsUpDown.Name = "RecoverDetailsUpDown";
            RecoverDetailsUpDown.Size = new Size(100, 23);
            RecoverDetailsUpDown.TabIndex = 15;
            RecoverDetailsUpDown.Visible = false;
            RecoverDetailsUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // RecoverDetailsLabel
            // 
            RecoverDetailsLabel.AutoSize = true;
            RecoverDetailsLabel.Location = new Point(64, 301);
            RecoverDetailsLabel.Name = "RecoverDetailsLabel";
            RecoverDetailsLabel.Size = new Size(89, 15);
            RecoverDetailsLabel.TabIndex = 14;
            RecoverDetailsLabel.Text = "Recover details:";
            RecoverDetailsLabel.Visible = false;
            // 
            // RevertCompressionUpDown
            // 
            RevertCompressionUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            RevertCompressionUpDown.Location = new Point(191, 270);
            RevertCompressionUpDown.Name = "RevertCompressionUpDown";
            RevertCompressionUpDown.Size = new Size(100, 23);
            RevertCompressionUpDown.TabIndex = 13;
            RevertCompressionUpDown.Visible = false;
            RevertCompressionUpDown.ValueChanged += ProteusSettingUpDown_ValueChanged;
            // 
            // RevertCompressionLabel
            // 
            RevertCompressionLabel.AutoSize = true;
            RevertCompressionLabel.Location = new Point(64, 272);
            RevertCompressionLabel.Name = "RevertCompressionLabel";
            RevertCompressionLabel.Size = new Size(114, 15);
            RevertCompressionLabel.TabIndex = 12;
            RevertCompressionLabel.Text = "Revert compression:";
            RevertCompressionLabel.Visible = false;
            // 
            // AutoCheckBox
            // 
            AutoCheckBox.AutoSize = true;
            AutoCheckBox.Location = new Point(64, 250);
            AutoCheckBox.Name = "AutoCheckBox";
            AutoCheckBox.Size = new Size(52, 19);
            AutoCheckBox.TabIndex = 11;
            AutoCheckBox.Text = "Auto";
            AutoCheckBox.UseVisualStyleBackColor = true;
            AutoCheckBox.Visible = false;
            AutoCheckBox.CheckedChanged += AutoCheckBox_CheckedChanged;
            // 
            // WeightLabel
            // 
            WeightLabel.AutoSize = true;
            WeightLabel.Location = new Point(7, 547);
            WeightLabel.Name = "WeightLabel";
            WeightLabel.Size = new Size(48, 15);
            WeightLabel.TabIndex = 29;
            WeightLabel.Text = "Weight:";
            WeightLabel.Visible = false;
            // 
            // WeightUpDown
            // 
            WeightUpDown.DecimalPlaces = 2;
            WeightUpDown.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            WeightUpDown.Location = new Point(64, 545);
            WeightUpDown.Name = "WeightUpDown";
            WeightUpDown.Size = new Size(100, 23);
            WeightUpDown.TabIndex = 30;
            WeightUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            WeightUpDown.Visible = false;
            WeightUpDown.ValueChanged += WeightUpDown_ValueChanged;
            // 
            // HeightUpDown
            // 
            HeightUpDown.Location = new Point(170, 8);
            HeightUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            HeightUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            HeightUpDown.Name = "HeightUpDown";
            HeightUpDown.Size = new Size(100, 23);
            HeightUpDown.TabIndex = 2;
            HeightUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            HeightUpDown.Visible = false;
            HeightUpDown.ValueChanged += HeightUpDown_ValueChanged;
            HeightUpDown.KeyDown += WidthHeightUpDown_KeyDown;
            // 
            // WidthUpDown
            // 
            WidthUpDown.Location = new Point(64, 8);
            WidthUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            WidthUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            WidthUpDown.Name = "WidthUpDown";
            WidthUpDown.Size = new Size(100, 23);
            WidthUpDown.TabIndex = 1;
            WidthUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            WidthUpDown.Visible = false;
            WidthUpDown.ValueChanged += WidthUpDown_ValueChanged;
            WidthUpDown.KeyDown += WidthHeightUpDown_KeyDown;
            // 
            // OutputResolutionTextBox
            // 
            OutputResolutionTextBox.Location = new Point(64, 198);
            OutputResolutionTextBox.Name = "OutputResolutionTextBox";
            OutputResolutionTextBox.ReadOnly = true;
            OutputResolutionTextBox.Size = new Size(121, 23);
            OutputResolutionTextBox.TabIndex = 10;
            OutputResolutionTextBox.Visible = false;
            // 
            // UpscaleFactorDropDownList
            // 
            UpscaleFactorDropDownList.DropDownStyle = ComboBoxStyle.DropDownList;
            UpscaleFactorDropDownList.FormattingEnabled = true;
            UpscaleFactorDropDownList.Items.AddRange(new object[] { "1x", "2x", "4x" });
            UpscaleFactorDropDownList.Location = new Point(191, 169);
            UpscaleFactorDropDownList.MaxDropDownItems = 3;
            UpscaleFactorDropDownList.Name = "UpscaleFactorDropDownList";
            UpscaleFactorDropDownList.Size = new Size(100, 23);
            UpscaleFactorDropDownList.TabIndex = 9;
            UpscaleFactorDropDownList.Visible = false;
            UpscaleFactorDropDownList.SelectedIndexChanged += UpscaleFactorDropDownList_SelectedIndexChanged;
            // 
            // UpscaleAlgorithmDropDownList
            // 
            UpscaleAlgorithmDropDownList.DropDownStyle = ComboBoxStyle.DropDownList;
            UpscaleAlgorithmDropDownList.FormattingEnabled = true;
            UpscaleAlgorithmDropDownList.Location = new Point(64, 169);
            UpscaleAlgorithmDropDownList.Name = "UpscaleAlgorithmDropDownList";
            UpscaleAlgorithmDropDownList.Size = new Size(121, 23);
            UpscaleAlgorithmDropDownList.TabIndex = 8;
            UpscaleAlgorithmDropDownList.Visible = false;
            UpscaleAlgorithmDropDownList.SelectedIndexChanged += UpscaleAlgorithmDropDownList_SelectedIndexChanged;
            // 
            // UpscaleLabel
            // 
            UpscaleLabel.AutoSize = true;
            UpscaleLabel.Location = new Point(7, 172);
            UpscaleLabel.Name = "UpscaleLabel";
            UpscaleLabel.Size = new Size(51, 15);
            UpscaleLabel.TabIndex = 7;
            UpscaleLabel.Text = "Upscale:";
            UpscaleLabel.Visible = false;
            // 
            // NoiseLabel
            // 
            NoiseLabel.AutoSize = true;
            NoiseLabel.Location = new Point(7, 119);
            NoiseLabel.Name = "NoiseLabel";
            NoiseLabel.Size = new Size(40, 15);
            NoiseLabel.TabIndex = 5;
            NoiseLabel.Text = "Noise:";
            NoiseLabel.Visible = false;
            // 
            // DownscaleAlgorithmDropDownList
            // 
            DownscaleAlgorithmDropDownList.DropDownStyle = ComboBoxStyle.DropDownList;
            DownscaleAlgorithmDropDownList.FormattingEnabled = true;
            DownscaleAlgorithmDropDownList.Items.AddRange(new object[] { "None", "Spline64", "Lanczos", "Bicubic", "Bilinear" });
            DownscaleAlgorithmDropDownList.Location = new Point(64, 37);
            DownscaleAlgorithmDropDownList.Name = "DownscaleAlgorithmDropDownList";
            DownscaleAlgorithmDropDownList.Size = new Size(121, 23);
            DownscaleAlgorithmDropDownList.TabIndex = 3;
            DownscaleAlgorithmDropDownList.Visible = false;
            DownscaleAlgorithmDropDownList.SelectedIndexChanged += DownscaleAlgorithmDropDownList_SelectedIndexChanged;
            // 
            // ResizeLabel
            // 
            ResizeLabel.AutoSize = true;
            ResizeLabel.Location = new Point(7, 10);
            ResizeLabel.Name = "ResizeLabel";
            ResizeLabel.Size = new Size(42, 15);
            ResizeLabel.TabIndex = 0;
            ResizeLabel.Text = "Resize:";
            ResizeLabel.Visible = false;
            // 
            // PresetDropDownList
            // 
            PresetDropDownList.DropDownStyle = ComboBoxStyle.DropDownList;
            PresetDropDownList.FormattingEnabled = true;
            PresetDropDownList.Items.AddRange(new object[] { "Input size", "Source size", "Aspect ratio corrected source size (upward)", "Aspect ratio corrected source size (downward)", "Fixed size", "Final size", "2/3 final size", "Half final size", "Quarter final size", "2/3 final size, never smallen than source", "Half final size, never small than source", "Quarter final size, never smaller than source", "2/3 input size", "Half input size", "Quarter input size", "Aspect ratio corrected input size (upward)", "Aspect ratio corrected input size (downward)" });
            PresetDropDownList.Location = new Point(64, 66);
            PresetDropDownList.Name = "PresetDropDownList";
            PresetDropDownList.Size = new Size(275, 23);
            PresetDropDownList.TabIndex = 4;
            PresetDropDownList.Visible = false;
            PresetDropDownList.SelectedIndexChanged += PresetDropDownList_SelectedIndexChanged;
            // 
            // NoiseUpDown
            // 
            NoiseUpDown.DecimalPlaces = 2;
            NoiseUpDown.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            NoiseUpDown.Location = new Point(64, 117);
            NoiseUpDown.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            NoiseUpDown.Name = "NoiseUpDown";
            NoiseUpDown.Size = new Size(100, 23);
            NoiseUpDown.TabIndex = 6;
            NoiseUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            NoiseUpDown.Visible = false;
            NoiseUpDown.ValueChanged += NoiseUpDown_ValueChanged;
            // 
            // OpenProjectDialog
            // 
            OpenProjectDialog.Filter = "Lab files (*.tvlab)|*.tvlab|All files (*.*)|*.*";
            // 
            // SaveProjectDialog
            // 
            SaveProjectDialog.Filter = "Lab files (*.tvlab)|*.tvlab|All files (*.*)|*.*";
            // 
            // OpenVideoDialog
            // 
            OpenVideoDialog.Filter = "All video files (*.avi;*.mkv;*.mp4;*.avs)|*.avi;*.mkv;*.mp4;*.avs|All files (*.*)|*.*";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1231, 949);
            Controls.Add(TableLayoutPanel);
            Controls.Add(StatusStrip);
            Controls.Add(ToolStrip);
            Controls.Add(MenuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Topaz Video AI Lab";
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            TableLayoutPanel.ResumeLayout(false);
            PreviewControlPanel.ResumeLayout(false);
            PreviewControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewScrubTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)FullScrubTrackBar).EndInit();
            GraphPanel.ResumeLayout(false);
            StepPropertiesPanel.ResumeLayout(false);
            StepPropertiesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)RecoverOriginalDetailsUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)OffsetYUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)OffsetXUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)AntiAliasDeblurUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)DehaloUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ReduceNoiseUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)SharpenUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)RecoverDetailsUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)RevertCompressionUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)WeightUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)HeightUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)WidthUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NoiseUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip ToolStrip;
        private MenuStrip MenuStrip;
        private ToolStripMenuItem FileMenuItem;
        private ToolStripMenuItem NewMenuItem;
        private ToolStripMenuItem OpenMenuItem;
        private ToolStripMenuItem SaveMenuItem;
        private ToolStripSeparator Break1MenuItem;
        private ToolStripMenuItem ExitMenuItem;
        private StatusStrip StatusStrip;
        private TableLayoutPanel TableLayoutPanel;
        private Panel GraphPanel;
        private Panel PreviewControlPanel;
        private Panel StepPropertiesPanel;
        private Label NoiseLabel;
        private ComboBox DownscaleAlgorithmDropDownList;
        private Label ResizeLabel;
        private ComboBox PresetDropDownList;
        private NumericUpDown NoiseUpDown;
        private TextBox OutputResolutionTextBox;
        private ComboBox UpscaleFactorDropDownList;
        private ComboBox UpscaleAlgorithmDropDownList;
        private Label UpscaleLabel;
        private ComboBox RenderLengthComboBox;
        private Button RenderButton;
        private TrackBar FullScrubTrackBar;
        private NumericUpDown HeightUpDown;
        private NumericUpDown WidthUpDown;
        private OpenFileDialog OpenProjectDialog;
        private SaveFileDialog SaveProjectDialog;
        internal OpenFileDialog OpenVideoDialog;
        private Label WeightLabel;
        private NumericUpDown WeightUpDown;
        private ToolStripMenuItem SaveAsMenuItem;
        private TrackBar PreviewScrubTrackBar;
        internal Cybercraft.Common.WinForms.PaintPanel GraphPaintPanel;
        private NumericUpDown AntiAliasDeblurUpDown;
        private Label AntiAliasDeblurLabel;
        private NumericUpDown DehaloUpDown;
        private Label DehaloLabel;
        private NumericUpDown ReduceNoiseUpDown;
        private Label ReduceNoiseLabel;
        private NumericUpDown SharpenUpDown;
        private Label SharpenLabel;
        private NumericUpDown RecoverDetailsUpDown;
        private Label RecoverDetailsLabel;
        private NumericUpDown RevertCompressionUpDown;
        private Label RevertCompressionLabel;
        private CheckBox AutoCheckBox;
        private NumericUpDown OffsetYUpDown;
        private NumericUpDown OffsetXUpDown;
        private Label OffsetLabel;
        private NumericUpDown RecoverOriginalDetailsUpDown;
        private Label RecoverOriginalDetailsLabel;
        private ComboBox NoisePresetDropDownList;
    }
}