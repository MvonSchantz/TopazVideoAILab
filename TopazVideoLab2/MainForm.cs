using System.Reflection;
using Cybercraft.Common.WinForms;
using Cybercraft.Common.WinForms.Gdi;
using TopazVideoLab.Project;
using TopazVideoLab2.Models;
using VideoProcessingLib.AviSynth;
using ColorSpace = TopazVideoLab.Project.ColorSpace;
using Size = VideoProcessingLib.AviSynth.Size;

namespace TopazVideoLab2
{
    public partial class MainForm : Form, IMainForm
    {
        public Project Project { get; } = new Project();

        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;

        public GraphTemplate[] GraphTemplates { get; } = null;// = new GraphTemplate[7];

        public GraphTemplate DragTemplate { get; private set; } = null;
        public GraphArrow DragArrow { get; private set; } = null;

        public List<GraphNode> GraphNodes { get; } = new List<GraphNode>();

        ICombination[] IMainForm.Combinations => GraphNodes.Cast<ICombination>().ToArray();

        public GraphNode SelectedNode => GraphNodes.FirstOrDefault(n => n.IsSelected);
        public GraphNodeInput SelectedInput => GraphNodes.SelectMany(n => n.Inputs).FirstOrDefault(i => i.IsSelected);

        public PreviewForm PreviewForm { get; }

        public int PreviewFrame
        {
            get => FullScrubTrackBar.Value;
            set
            {
                if (FullScrubTrackBar.Maximum <= 1)
                {
                    LoadSourceVideo();
                }
                FullScrubTrackBar.Value = value;
            }
        }

        public int PreviewLength
        {
            get => PreviewScrubTrackBar.Maximum + 1;
            set => RenderLengthComboBox.Text = value.ToInvariant();
        }

        public Version Version => Cybercraft.Common.Version.GetAssemblyVersion();

        private static readonly ModelManager ModelManagerInstance = new ModelManager();
        IModelManager IMainForm.ModelManager => ModelManagerInstance;

        public MainForm()
        {
            InitializeComponent();

            WeightLabel.Top = ResizeLabel.Top;
            WeightUpDown.Top = WidthUpDown.Top;

            RenderLengthComboBox.SelectedIndex = 1;

            UpscaleAlgorithmDropDownList.Items.AddRange(ModelManager.Names.Cast<object>().ToArray());

            var templates = ModelManager.Templates;
            GraphTemplates = new GraphTemplate[templates.Length + 2];

            GraphTemplates[0] = new GraphTemplate(this, 20, 20)
            { IsSource = true, UpscaleAlgorithm = UpscaleAlgorithm.None };

            for (int i = 0; i < templates.Length; i++)
            {
                GraphTemplates[i + 1] = new GraphTemplate(this, 20, 20 + 70 * (i + 1))
                {
                    UpscaleAlgorithm = templates[i].UpscaleAlgorithm,
                    UpscaleFactor = templates[i].DefaultFactor,
                };
            }

            GraphTemplates[GraphTemplates.Length - 1] = new GraphTemplate(this, 20, 30 + 70 * (templates.Length + 1))
            { UpscaleAlgorithm = UpscaleAlgorithm.None, };




            GraphPaintPanel_SizeChanged(null, null);

            (GraphPaintPanel as Control).KeyUp += new KeyEventHandler(GraphPaintPanel_KeyUp);

            PreviewForm = new PreviewForm(this);
            PreviewForm.Show(this);
            PreviewForm.Left = this.Right;
            PreviewForm.Top = this.Top;

            //TopazProcess.Run(@"k:\NoAV\Crime Traveller - 1x01 - Jeff Slade and the Loop of Infinity.avi", @"k:\NoAV\t\");

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                LoadProject(args[1]);
            }
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            Project.Clear();
            GraphNodes.Clear();

            GraphPaintPanel.Invalidate();

            Text = "Topaz Video AI Lab";
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (Project.ProjectFileName == null)
            {
                SaveAsMenuItem_Click(sender, e);
                return;
            }

            var xml = Project.SaveToXml(this);
            xml.SourceVideo.Value = Path.GetRelativePath(Path.GetDirectoryName(Project.ProjectFileName), xml.SourceVideo.Value);
            xml.SaveToFile(Project.ProjectFileName);
        }

        public void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveProjectDialog.FileName = Project.ProjectFileName;
            if (SaveProjectDialog.ShowDialog(this) == DialogResult.OK)
            {
                Project.ProjectFileName = SaveProjectDialog.FileName;

                var xml = Project.SaveToXml(this);
                xml.SourceVideo.Value = Path.GetRelativePath(Path.GetDirectoryName(Project.ProjectFileName), xml.SourceVideo.Value);
                xml.SaveToFile(Project.ProjectFileName);

                Text = "Topaz Video AI Lab - " + Path.GetFileName(Project.ProjectFileName);
            }
        }

        private void LoadSourceVideo()
        {
            using (var avi = new AviReader(Project.SourceVideo, true))
            {
                FullScrubTrackBar.Value = 0;
                FullScrubTrackBar.Maximum = avi.Length - 1;
                PreviewScrubTrackBar.Maximum = 1;
                PreviewScrubTrackBar.Value = 0;

                Project.Frames = avi.Length;
                Project.InputSize = avi.FrameSize;
                Project.Framerate = avi.FrameRate;
            }
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenProjectDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadProject(OpenProjectDialog.FileName);
            }
        }

        private void LoadProject(string fileName)
        {
            Project.Clear();
            GraphNodes.Clear();
            FullScrubTrackBar.Value = 0;
            FullScrubTrackBar.Maximum = 1;
            PreviewScrubTrackBar.Maximum = 1;
            PreviewScrubTrackBar.Value = 0;

            TopazVideoLabProject xml;
            try
            {
                xml = TopazVideoLabProject.LoadFromFile(fileName);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.Message + "\n\n" + ex.InnerException : ex.Message;
                MessageBox.Show(this, message, "Error loading project");

                GraphPaintPanel.Invalidate();
                Text = "Topaz Video AI Lab";
                return;
            }

            Project.ProjectFileName = fileName;
            Project.LoadFromXml(this, xml);

            RenderLengthComboBox_TextChanged(null, null);

            //LoadSourceVideo();

            GraphPaintPanel.Invalidate();

            Text = "Topaz Video AI Lab - " + Path.GetFileName(fileName);
        }

        public void LoadSource()
        {
            if (OpenVideoDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (var avi = new AviReader(OpenVideoDialog.FileName, true))
                {
                    FullScrubTrackBar.Value = 0;
                    FullScrubTrackBar.Maximum = avi.Length - 1;
                    PreviewScrubTrackBar.Value = 0;

                    Project.Frames = avi.Length;
                    Project.InputSize = avi.FrameSize;
                    Project.Framerate = avi.FrameRate;
                    Project.SourceVideo = OpenVideoDialog.FileName;

                    var propertiesForm = new ProjectPropertiesForm();
                    propertiesForm.CancelButton = null;
                    propertiesForm.CancelButtonControl.Enabled = false;
                    propertiesForm.ControlBox = false;
                    propertiesForm.InputWidthTextBox.Text = avi.FrameSize.Width.ToInvariant();
                    propertiesForm.InputHeightTextBox.Text = avi.FrameSize.Height.ToInvariant();
                    propertiesForm.GuessAspectRatioAndColorSpace();
                    propertiesForm.ShowDialog(this);

                    switch (propertiesForm.AspectRatioDropdownList.SelectedIndex)
                    {
                        case 0:
                            Project.AspectRatio = AspectRatio.Ratio4by3;
                            break;
                        case 1:
                            Project.AspectRatio = AspectRatio.Ratio16by9;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    switch (propertiesForm.ColorSpaceDropDownList.SelectedIndex)
                    {
                        case 0:
                            Project.ColorSpace = ColorSpace.Rec601;
                            break;
                        case 1:
                            Project.ColorSpace = ColorSpace.Rec709;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    switch (propertiesForm.DeinterlaceDropDownList.SelectedIndex)
                    {
                        case 0:
                            Project.Interlace = Interlace.Progressive;
                            break;
                        case 1:
                            Project.Interlace = Interlace.IVTC;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    Project.OutputSize = new Size(int.Parse(propertiesForm.OutputWidthTextBox.Text), int.Parse(propertiesForm.OutputHeightTextBox.Text));
                }

                foreach (var graphNode in GraphNodes)
                {
                    CalculateResize(graphNode, false);
                }
            }
        }

        ICombination IMainForm.AddCombination(bool isSource)
        {
            var combination = new GraphNode(this, 0, 0, isSource);
            GraphNodes.Add(combination);
            return combination;
        }

        private void GraphPaintPanel_PaintGdi(object sender, PaintGdiEventArgs e)
        {
            var area = new RECT(0, 0, e.Width, e.Height);
            using (var dc = GdiDc.FromHandle(e.Dc))
            {
                using (var backgroundBrush = new GdiBrush(BackColor))
                {
                    dc.FillRect(0, 0, e.Width, e.Height, backgroundBrush);
                }

                using (var graphDc = new GraphDc(dc, OffsetX, OffsetY))
                {
                    foreach (var nodeInput in GraphNodes.SelectMany(n => n.Inputs))
                    {
                        var arrow = nodeInput;
                        arrow.MoveStart((nodeInput.Source.Left + nodeInput.Source.Right) / 2, nodeInput.Source.Bottom);
                        arrow.MoveEnd((nodeInput.Target.Left + nodeInput.Target.Right) / 2, nodeInput.Target.Top);

                        arrow.Draw(graphDc);
                    }

                    foreach (var graphNode in GraphNodes)
                    {
                        graphNode.Draw(graphDc);
                    }

                    foreach (var graphTemplate in GraphTemplates)
                    {
                        graphTemplate.Draw(graphDc);
                    }

                    DragTemplate?.Draw(graphDc);

                    DragArrow?.Draw(graphDc);
                }
            }
        }

        private void GraphPaintPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (var graphTemplate in GraphTemplates)
            {
                graphTemplate.Left = GraphPaintPanel.Width - graphTemplate.Width - 20;
            }

            GraphPaintPanel.Invalidate();
        }

        public GraphComponent ComponentUnderMouse(Point mouse)
        {
            foreach (var template in GraphTemplates)
            {
                if (template.IsInside(mouse.X, mouse.Y))
                {
                    return template;
                }
            }

            foreach (var node in GraphNodes)
            {
                if (node.IsInside(mouse.X - OffsetX, mouse.Y - OffsetY))
                {
                    return node;
                }
            }

            foreach (var nodeInput in GraphNodes.SelectMany(n => n.Inputs))
            {
                if (nodeInput.IsInside(mouse.X - OffsetX, mouse.Y - OffsetY))
                {
                    return nodeInput;
                }
            }

            return null;
        }

        public bool IsMouseDown { get; private set; } = false;
        public GraphComponent MouseComponent { get; private set; } = null;
        public MouseButtons ButtonDown { get; private set; } = MouseButtons.None;

        private Point MouseComponentDelta;
        private Point PanOrigin;
        private Point StartOffset;
        public GraphNode ArrowStartNode { get; private set; } = null;

        private void GraphPaintPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (((Control)sender).Capture) { }

                IsMouseDown = true;
                ButtonDown = MouseButtons.Right;
                PanOrigin = e.Location;
                StartOffset = new Point(OffsetX, OffsetY);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                GraphPaintPanel.Focus();

                foreach (var node in GraphNodes)
                {
                    if (!IsCtrlPressed)
                    {
                        node.IsSelected = false;
                    }

                    foreach (var nodeInput in node.Inputs)
                    {
                        nodeInput.IsSelected = false;
                    }
                }
                ShowComponentSettings(null);

                var component = ComponentUnderMouse(e.Location);
                if (component != null)
                {
                    ButtonDown = MouseButtons.Left;
                    if (((Control)sender).Capture) { }
                    IsMouseDown = true;

                    if (component is GraphTemplate template)
                    {
                        DragTemplate = new GraphTemplate(this, component.Left, component.Top)
                        {
                            IsSource = template.IsSource,
                            UpscaleAlgorithm = template.UpscaleAlgorithm,
                            UpscaleFactor = template.UpscaleFactor,
                            IsSelected = true,
                        };
                        MouseComponent = DragTemplate;
                        MouseComponentDelta = new Point(e.Location.X - component.Left, e.Location.Y - component.Top);
                    }
                    else if (component is GraphNode node)
                    {
                        node.IsSelected = IsCtrlPressed ? !node.IsSelected : true;
                        if (e.Location.Y - OffsetY - component.Top <= 2 * component.Height / 3)
                        {
                            MouseComponent = component;
                            MouseComponentDelta = new Point(e.Location.X - component.Left, e.Location.Y - component.Top);
                        }
                        else
                        {
                            DragArrow = new GraphArrow(this, (component.Left + component.Right) / 2, component.Bottom, e.Location.X, e.Location.Y, false);
                            MouseComponent = DragArrow;
                            ArrowStartNode = node;
                        }

                        ShowComponentSettings(component);

                        component.OnMouseDown();
                    }
                    else if (component is GraphNodeInput nodeInput)
                    {
                        nodeInput.IsSelected = true;
                        ShowComponentSettings(component);
                    }


                }

                GraphPaintPanel.Invalidate();
            }
        }

        private bool isUpdatingComponentSettings = false;

        private void ShowComponentSettings(GraphComponent component)
        {
            isUpdatingComponentSettings = true;

            if (component is GraphNode && GraphNodes.Count(n => n.IsSelected) != 1)
            {
                component = null;
            }

            if (component is GraphNode)
            {
                component = GraphNodes.FirstOrDefault(n => n.IsSelected);
                var node = (GraphNode)component;

                var model = ModelManager.GetModel(node.UpscaleAlgorithm);

                if (node.UpscaleAlgorithm != UpscaleAlgorithm.None)
                {
                    ResizeLabel.Visible = true;
                    WidthUpDown.Visible = true;
                    HeightUpDown.Visible = true;
                    PresetDropDownList.Visible = true;
                    DownscaleAlgorithmDropDownList.Visible = true;

                    NoiseLabel.Visible = true;
                    NoiseUpDown.Visible = true;
                    NoisePresetDropDownList.Visible = true;

                    UpscaleLabel.Visible = true;
                    UpscaleAlgorithmDropDownList.Visible = true;
                    UpscaleFactorDropDownList.Visible = true;
                    OutputResolutionTextBox.Visible = true;

                    if (model.HasDetailedSettings)
                    {
                        AutoCheckBox.Visible = true;
                        RevertCompressionLabel.Visible = true;
                        RevertCompressionUpDown.Visible = true;
                        RecoverDetailsLabel.Visible = true;
                        RecoverDetailsUpDown.Visible = true;
                        SharpenLabel.Visible = true;
                        SharpenUpDown.Visible = true;
                        ReduceNoiseLabel.Visible = true;
                        ReduceNoiseUpDown.Visible = true;
                        DehaloLabel.Visible = true;
                        DehaloUpDown.Visible = true;
                        AntiAliasDeblurLabel.Visible = true;
                        AntiAliasDeblurUpDown.Visible = true;

                        if (model.HasOffsetSettings)
                        {
                            //OffsetLabel.Visible = true;
                            //OffsetXUpDown.Visible = true;
                            //OffsetYUpDown.Visible = true;
                        }
                        else
                        {
                            OffsetLabel.Visible = false;
                            OffsetXUpDown.Visible = false;
                            OffsetYUpDown.Visible = false;
                        }

                        AutoCheckBox.Checked = node.Auto;
                        RevertCompressionUpDown.Value = node.RevertCompression;
                        RecoverDetailsUpDown.Value = node.RecoverDetails;
                        SharpenUpDown.Value = node.Sharpen;
                        ReduceNoiseUpDown.Value = node.ReduceNoise;
                        DehaloUpDown.Value = node.Dehalo;
                        AntiAliasDeblurUpDown.Value = node.AntiAliasDeblur;
                        OffsetXUpDown.Value = (decimal)(node.OffsetX / 4.0);
                        OffsetYUpDown.Value = (decimal)(node.OffsetY / 4.0);
                    }
                    else
                    {
                        AutoCheckBox.Visible = false;
                        RevertCompressionLabel.Visible = false;
                        RevertCompressionUpDown.Visible = false;
                        RecoverDetailsLabel.Visible = false;
                        RecoverDetailsUpDown.Visible = false;
                        SharpenLabel.Visible = false;
                        SharpenUpDown.Visible = false;
                        ReduceNoiseLabel.Visible = false;
                        ReduceNoiseUpDown.Visible = false;
                        DehaloLabel.Visible = false;
                        DehaloUpDown.Visible = false;
                        AntiAliasDeblurLabel.Visible = false;
                        AntiAliasDeblurUpDown.Visible = false;
                        OffsetLabel.Visible = false;
                        OffsetXUpDown.Visible = false;
                        OffsetYUpDown.Visible = false;
                    }

                    if (model.HasRecoverOriginalSettings)
                    {
                        RecoverOriginalDetailsLabel.Visible = true;
                        RecoverOriginalDetailsUpDown.Visible = true;
                        RecoverOriginalDetailsUpDown.Value = node.RecoverOriginalDetails;
                    }
                    else
                    {
                        RecoverOriginalDetailsLabel.Visible = false;
                        RecoverOriginalDetailsUpDown.Visible = false;
                    }

                    WeightLabel.Visible = false;
                    WeightUpDown.Visible = false;

                    switch (node.ResizeAlgorithm)
                    {
                        case ResizeAlgorithm.None:
                            DownscaleAlgorithmDropDownList.SelectedIndex = 0;
                            break;
                        case ResizeAlgorithm.Spline64:
                            DownscaleAlgorithmDropDownList.SelectedIndex = 1;
                            break;
                        case ResizeAlgorithm.Lanczos:
                            DownscaleAlgorithmDropDownList.SelectedIndex = 2;
                            break;
                        case ResizeAlgorithm.Bicubic:
                            DownscaleAlgorithmDropDownList.SelectedIndex = 3;
                            break;
                        case ResizeAlgorithm.Bilinear:
                            DownscaleAlgorithmDropDownList.SelectedIndex = 4;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    switch (node.ResizePreset)
                    {
                        case ResizePreset.InputSize:
                            PresetDropDownList.SelectedIndex = 0;
                            break;
                        case ResizePreset.SourceSize:
                            PresetDropDownList.SelectedIndex = 1;
                            break;
                        case ResizePreset.AspectCorrectedSourceSizeUpward:
                            PresetDropDownList.SelectedIndex = 2;
                            break;
                        case ResizePreset.AspectCorrectedSourceSizeDownward:
                            PresetDropDownList.SelectedIndex = 3;
                            break;
                        case ResizePreset.FixedSize:
                            PresetDropDownList.SelectedIndex = 4;
                            break;
                        case ResizePreset.FinalSize:
                            PresetDropDownList.SelectedIndex = 5;
                            break;
                        case ResizePreset.TwoThirdsFinalSize:
                            PresetDropDownList.SelectedIndex = 6;
                            break;
                        case ResizePreset.HalfFinalSize:
                            PresetDropDownList.SelectedIndex = 7;
                            break;
                        case ResizePreset.QuarterFinalSize:
                            PresetDropDownList.SelectedIndex = 8;
                            break;
                        case ResizePreset.TwoThirdsFinalSizeLimitBySource:
                            PresetDropDownList.SelectedIndex = 9;
                            break;
                        case ResizePreset.HalfFinalSizeLimitBySource:
                            PresetDropDownList.SelectedIndex = 10;
                            break;
                        case ResizePreset.QuarterFinalSizeLimitBySource:
                            PresetDropDownList.SelectedIndex = 11;
                            break;
                        case ResizePreset.TwoThirdsInputSize:
                            PresetDropDownList.SelectedIndex = 12;
                            break;
                        case ResizePreset.HalfInputSize:
                            PresetDropDownList.SelectedIndex = 13;
                            break;
                        case ResizePreset.QuarterInputSize:
                            PresetDropDownList.SelectedIndex = 14;
                            break;
                        case ResizePreset.AspectCorrectedInputSizeUpward:
                            PresetDropDownList.SelectedIndex = 15;
                            break;
                        case ResizePreset.AspectCorrectedInputSizeDownward:
                            PresetDropDownList.SelectedIndex = 16;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    WidthUpDown.Text = node.Resize.Width.ToInvariant();
                    HeightUpDown.Text = node.Resize.Height.ToInvariant();

                    NoiseUpDown.Value = (decimal)node.Noise;

                    switch (node.NoisePreset)
                    {
                        case NoisePreset.QtgmcPlacebo:
                            NoisePresetDropDownList.SelectedIndex = 0;
                            break;
                        case NoisePreset.QtgmcVerySlow:
                            NoisePresetDropDownList.SelectedIndex = 1;
                            break;
                        case NoisePreset.QtgmcSlower:
                            NoisePresetDropDownList.SelectedIndex = 2;
                            break;
                        case NoisePreset.QtgmcSlow:
                            NoisePresetDropDownList.SelectedIndex = 3;
                            break;
                        case NoisePreset.QtgmcMedium:
                            NoisePresetDropDownList.SelectedIndex = 4;
                            break;
                        case NoisePreset.QtgmcFast:
                            NoisePresetDropDownList.SelectedIndex = 5;
                            break;
                        case NoisePreset.QtgmcFaster:
                            NoisePresetDropDownList.SelectedIndex = 6;
                            break;
                        case NoisePreset.QtgmcVeryFast:
                            NoisePresetDropDownList.SelectedIndex = 7;
                            break;
                        case NoisePreset.QtgmcSuperFast:
                            NoisePresetDropDownList.SelectedIndex = 8;
                            break;
                        case NoisePreset.QtgmcUltraFast:
                            NoisePresetDropDownList.SelectedIndex = 9;
                            break;
                        case NoisePreset.QtgmcDraft:
                            NoisePresetDropDownList.SelectedIndex = 10;
                            break;
                        case NoisePreset.FilmGrain005:
                            NoisePresetDropDownList.SelectedIndex = 11;
                            break;
                        case NoisePreset.FilmGrain010:
                            NoisePresetDropDownList.SelectedIndex = 12;
                            break;
                        case NoisePreset.FilmGrain015:
                            NoisePresetDropDownList.SelectedIndex = 13;
                            break;
                        case NoisePreset.FilmGrain020:
                            NoisePresetDropDownList.SelectedIndex = 14;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    switch (node.UpscaleAlgorithm)
                    {
                        case UpscaleAlgorithm.None:
                            break;
                        default:
                            int order = ModelManager.GetOrder(node.UpscaleAlgorithm);
                            if (order < 0)
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            UpscaleAlgorithmDropDownList.SelectedIndex = order;
                            break;
                    }

                    switch (node.UpscaleFactor)
                    {
                        case 1:
                            UpscaleFactorDropDownList.SelectedIndex = 0;
                            break;
                        case 2:
                            UpscaleFactorDropDownList.SelectedIndex = 1;
                            break;
                        case 4:
                            UpscaleFactorDropDownList.SelectedIndex = 2;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    OutputResolutionTextBox.Text = node.OutputSize.ToString();
                }
                else
                {
                    ResizeLabel.Visible = false;
                    WidthUpDown.Visible = false;
                    HeightUpDown.Visible = false;
                    PresetDropDownList.Visible = false;
                    DownscaleAlgorithmDropDownList.Visible = false;

                    NoiseLabel.Visible = false;
                    NoiseUpDown.Visible = false;
                    NoisePresetDropDownList.Visible = false;

                    UpscaleLabel.Visible = false;
                    UpscaleAlgorithmDropDownList.Visible = false;
                    UpscaleFactorDropDownList.Visible = false;
                    OutputResolutionTextBox.Visible = false;

                    AutoCheckBox.Visible = false;
                    RevertCompressionLabel.Visible = false;
                    RevertCompressionUpDown.Visible = false;
                    RecoverDetailsLabel.Visible = false;
                    RecoverDetailsUpDown.Visible = false;
                    SharpenLabel.Visible = false;
                    SharpenUpDown.Visible = false;
                    ReduceNoiseLabel.Visible = false;
                    ReduceNoiseUpDown.Visible = false;
                    DehaloLabel.Visible = false;
                    DehaloUpDown.Visible = false;
                    AntiAliasDeblurLabel.Visible = false;
                    AntiAliasDeblurUpDown.Visible = false;
                    OffsetLabel.Visible = false;
                    OffsetXUpDown.Visible = false;
                    OffsetYUpDown.Visible = false;

                    RecoverOriginalDetailsLabel.Visible = false;
                    RecoverOriginalDetailsUpDown.Visible = false;

                    WeightLabel.Visible = false;
                    WeightUpDown.Visible = false;
                }

            }
            else if (component is GraphNodeInput input)
            {
                ResizeLabel.Visible = false;
                WidthUpDown.Visible = false;
                HeightUpDown.Visible = false;
                PresetDropDownList.Visible = false;
                DownscaleAlgorithmDropDownList.Visible = false;

                NoiseLabel.Visible = false;
                NoiseUpDown.Visible = false;
                NoisePresetDropDownList.Visible = false;

                UpscaleLabel.Visible = false;
                UpscaleAlgorithmDropDownList.Visible = false;
                UpscaleFactorDropDownList.Visible = false;
                OutputResolutionTextBox.Visible = false;

                AutoCheckBox.Visible = false;
                RevertCompressionLabel.Visible = false;
                RevertCompressionUpDown.Visible = false;
                RecoverDetailsLabel.Visible = false;
                RecoverDetailsUpDown.Visible = false;
                SharpenLabel.Visible = false;
                SharpenUpDown.Visible = false;
                ReduceNoiseLabel.Visible = false;
                ReduceNoiseUpDown.Visible = false;
                DehaloLabel.Visible = false;
                DehaloUpDown.Visible = false;
                AntiAliasDeblurLabel.Visible = false;
                AntiAliasDeblurUpDown.Visible = false;
                OffsetLabel.Visible = false;
                OffsetXUpDown.Visible = false;
                OffsetYUpDown.Visible = false;

                RecoverOriginalDetailsLabel.Visible = false;
                RecoverOriginalDetailsUpDown.Visible = false;

                WeightLabel.Visible = true;
                WeightUpDown.Visible = true;

                WeightUpDown.Value = (decimal)input.Weight;
            }
            else
            {
                ResizeLabel.Visible = false;
                WidthUpDown.Visible = false;
                HeightUpDown.Visible = false;
                PresetDropDownList.Visible = false;
                DownscaleAlgorithmDropDownList.Visible = false;

                NoiseLabel.Visible = false;
                NoiseUpDown.Visible = false;
                NoisePresetDropDownList.Visible = false;

                UpscaleLabel.Visible = false;
                UpscaleAlgorithmDropDownList.Visible = false;
                UpscaleFactorDropDownList.Visible = false;
                OutputResolutionTextBox.Visible = false;

                AutoCheckBox.Visible = false;
                RevertCompressionLabel.Visible = false;
                RevertCompressionUpDown.Visible = false;
                RecoverDetailsLabel.Visible = false;
                RecoverDetailsUpDown.Visible = false;
                SharpenLabel.Visible = false;
                SharpenUpDown.Visible = false;
                ReduceNoiseLabel.Visible = false;
                ReduceNoiseUpDown.Visible = false;
                DehaloLabel.Visible = false;
                DehaloUpDown.Visible = false;
                AntiAliasDeblurLabel.Visible = false;
                AntiAliasDeblurUpDown.Visible = false;
                OffsetLabel.Visible = false;
                OffsetXUpDown.Visible = false;
                OffsetYUpDown.Visible = false;

                RecoverOriginalDetailsLabel.Visible = false;
                RecoverOriginalDetailsUpDown.Visible = false;

                WeightLabel.Visible = false;
                WeightUpDown.Visible = false;
            }

            isUpdatingComponentSettings = false;
        }

        private void GraphPaintPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                if (DragTemplate is GraphTemplate dragTemplate)
                {
                    foreach (var graphNode in GraphNodes)
                    {
                        graphNode.IsSelected = false;
                        foreach (var input in graphNode.Inputs)
                        {
                            input.IsSelected = false;
                        }
                    }

                    var newNode = new GraphNode(this, dragTemplate.Left - OffsetX, dragTemplate.Top - OffsetY, dragTemplate.IsSource)
                    {
                        IsSource = dragTemplate.IsSource,
                        Resize = new Size(1, 1),
                        ResizeAlgorithm = ResizeAlgorithm.Spline64,
                        UpscaleAlgorithm = dragTemplate.UpscaleAlgorithm,
                        UpscaleFactor = dragTemplate.UpscaleFactor,
                        IsSelected = true,
                    };
                    var model = ModelManager.GetModel(newNode.UpscaleAlgorithm);
                    if (model != null && model.HasDetailedSettings)
                    {
                        newNode.RevertCompression = 0;
                        newNode.RecoverDetails = 10;
                        newNode.Sharpen = 15;
                        newNode.ReduceNoise = 0;
                        newNode.Dehalo = 0;
                        newNode.AntiAliasDeblur = 10;
                        newNode.Auto = false;
                    }
                    GraphNodes.Add(newNode);

                    ShowComponentSettings(newNode);

                }
                else if (MouseComponent is GraphNode node)
                {
                    GraphNodes.Remove(node);
                    GraphNodes.Add(node);
                }
                else if (DragArrow != null)
                {
                    foreach (var node2 in GraphNodes)
                    {
                        if (ArrowStartNode != node2 && node2.IsHighlighted)
                        {
                            node2.Inputs.Add(new GraphNodeInput(ArrowStartNode, node2));
                            node2.IsHighlighted = false;
                            foreach (var graphNodeInput in node2.Inputs)
                            {
                                graphNodeInput.Weight = 1.0f / node2.Inputs.Count;
                            }
                            CalculateResize(node2, false);
                            break;
                        }
                    }


                }

                MouseComponent?.OnMouseUp();
                Capture = false;
                IsMouseDown = false;
                ButtonDown = MouseButtons.None;
                MouseComponent = null;
                DragTemplate = null;
                DragArrow = null;
                ArrowStartNode = null;
                MouseComponentDelta = Point.Empty;
                PanOrigin = Point.Empty;
                StartOffset = Point.Empty;
                GraphPaintPanel.Invalidate();
            }
        }

        private void GraphPaintPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (IsMouseDown && ButtonDown == MouseButtons.Left)
            {
                if (MouseComponent is GraphNode)
                {
                    MouseComponent.OnMouseDoubleClick();
                }
                else if (MouseComponent is GraphArrow)
                {
                    ArrowStartNode.OnMouseDoubleClick();
                }
            }
        }

        private void GraphPaintPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                if (ButtonDown == MouseButtons.Left)
                {
                    if (MouseComponent != null)
                    {
                        if (MouseComponent is GraphArrow arrow)
                        {
                            arrow.MoveEnd(e.Location.X - OffsetX, e.Location.Y - OffsetY);

                            foreach (var node2 in GraphNodes)
                            {
                                node2.IsHighlighted = false;
                            }
                            var mouseOverComponent = ComponentUnderMouse(e.Location);
                            if (mouseOverComponent != null && ArrowStartNode != mouseOverComponent && mouseOverComponent is GraphNode graphNode && !graphNode.IsSource)
                            {
                                graphNode.IsHighlighted = true;
                            }
                        }
                        else
                        {
                            MouseComponent.Left = e.Location.X - MouseComponentDelta.X;
                            MouseComponent.Top = e.Location.Y - MouseComponentDelta.Y;
                        }

                        if (MouseComponent is GraphNode node)
                        {
                            GraphNodes.Remove(node);
                            GraphNodes.Add(node);
                        }
                    }

                    GraphPaintPanel.Invalidate();
                }
                else if (ButtonDown == MouseButtons.Right)
                {
                    var newOffset = StartOffset;
                    var move = new System.Drawing.Size(e.Location.X - PanOrigin.X, e.Location.Y - PanOrigin.Y);
                    newOffset = Point.Add(newOffset, move);
                    OffsetX = newOffset.X;
                    OffsetY = newOffset.Y;
                    GraphPaintPanel.Invalidate();
                }
            }
        }

        private bool IsCtrlPressed => ModifierKeys.HasFlag(Keys.Control);

        private void GraphPaintPanel_KeyUp(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.ControlKey)
            {
                IsCtrlPressed = false;
            }*/
        }

        private void GraphPaintPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            /*if (e.KeyCode == Keys.ControlKey)
            {
                IsCtrlPressed = true;
                return;
            }*/


            if (e.KeyCode == Keys.Delete)
            {
                GraphNode nodeToDelete = null;

                foreach (var node in GraphNodes)
                {
                    if (node.IsSelected)
                    {
                        nodeToDelete = node;
                        foreach (var graphNode in GraphNodes)
                        {
                            int inputCount = graphNode.Inputs.Count;
                            graphNode.Inputs.RemoveAll(i => i.Source == node);
                            if (inputCount != graphNode.Inputs.Count)
                            {
                                CalculateResize(graphNode, false);
                            }
                        }

                        break;
                    }

                    int inputCount2 = node.Inputs.Count;
                    node.Inputs.RemoveAll(i => i.IsSelected);
                    if (inputCount2 != node.Inputs.Count)
                    {
                        CalculateResize(node, false);
                    }
                }

                if (nodeToDelete != null)
                {
                    GraphNodes.Remove(nodeToDelete);
                }

                GraphPaintPanel.Invalidate();
            }
        }

        private void UpscaleFactorDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            switch (UpscaleFactorDropDownList.SelectedIndex)
            {
                case 0:
                    node.UpscaleFactor = 1;
                    break;
                case 1:
                    node.UpscaleFactor = 2;
                    break;
                case 2:
                    node.UpscaleFactor = 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OutputResolutionTextBox.Text = node.OutputSize.ToString();

            GraphPaintPanel.Invalidate();
        }

        private void UpscaleAlgorithmDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            node.UpscaleAlgorithm = ModelManager.GetAlgorithm(UpscaleAlgorithmDropDownList.SelectedIndex);

            var model = ModelManager.GetModel(node.UpscaleAlgorithm);

            if (model.HasDetailedSettings)
            {
                AutoCheckBox.Visible = true;
                RevertCompressionLabel.Visible = true;
                RevertCompressionUpDown.Visible = true;
                RecoverDetailsLabel.Visible = true;
                RecoverDetailsUpDown.Visible = true;
                SharpenLabel.Visible = true;
                SharpenUpDown.Visible = true;
                ReduceNoiseLabel.Visible = true;
                ReduceNoiseUpDown.Visible = true;
                DehaloLabel.Visible = true;
                DehaloUpDown.Visible = true;
                AntiAliasDeblurLabel.Visible = true;
                AntiAliasDeblurUpDown.Visible = true;
                if (model.HasOffsetSettings)
                {
                    //OffsetLabel.Visible = true;
                    //OffsetXUpDown.Visible = true;
                    //OffsetYUpDown.Visible = true;
                }
                else
                {
                    OffsetLabel.Visible = false;
                    OffsetXUpDown.Visible = false;
                    OffsetYUpDown.Visible = false;
                }
            }
            else
            {
                AutoCheckBox.Visible = false;
                RevertCompressionLabel.Visible = false;
                RevertCompressionUpDown.Visible = false;
                RecoverDetailsLabel.Visible = false;
                RecoverDetailsUpDown.Visible = false;
                SharpenLabel.Visible = false;
                SharpenUpDown.Visible = false;
                ReduceNoiseLabel.Visible = false;
                ReduceNoiseUpDown.Visible = false;
                DehaloLabel.Visible = false;
                DehaloUpDown.Visible = false;
                AntiAliasDeblurLabel.Visible = false;
                AntiAliasDeblurUpDown.Visible = false;
                OffsetLabel.Visible = false;
                OffsetXUpDown.Visible = false;
                OffsetYUpDown.Visible = false;
            }


            GraphPaintPanel.Invalidate();
        }


        private void PresetDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            switch (PresetDropDownList.SelectedIndex)
            {
                case 0:
                    node.ResizePreset = ResizePreset.InputSize;
                    break;
                case 1:
                    node.ResizePreset = ResizePreset.SourceSize;
                    break;
                case 2:
                    node.ResizePreset = ResizePreset.AspectCorrectedSourceSizeUpward;
                    break;
                case 3:
                    node.ResizePreset = ResizePreset.AspectCorrectedSourceSizeDownward;
                    break;
                case 4:
                    node.ResizePreset = ResizePreset.FixedSize;
                    break;
                case 5:
                    node.ResizePreset = ResizePreset.FinalSize;
                    break;
                case 6:
                    node.ResizePreset = ResizePreset.TwoThirdsFinalSize;
                    break;
                case 7:
                    node.ResizePreset = ResizePreset.HalfFinalSize;
                    break;
                case 8:
                    node.ResizePreset = ResizePreset.QuarterFinalSize;
                    break;
                case 9:
                    node.ResizePreset = ResizePreset.TwoThirdsFinalSizeLimitBySource;
                    break;
                case 10:
                    node.ResizePreset = ResizePreset.HalfFinalSizeLimitBySource;
                    break;
                case 11:
                    node.ResizePreset = ResizePreset.QuarterFinalSizeLimitBySource;
                    break;
                case 12:
                    node.ResizePreset = ResizePreset.TwoThirdsInputSize;
                    break;
                case 13:
                    node.ResizePreset = ResizePreset.HalfInputSize;
                    break;
                case 14:
                    node.ResizePreset = ResizePreset.QuarterInputSize;
                    break;
                case 15:
                    node.ResizePreset = ResizePreset.AspectCorrectedInputSizeUpward;
                    break;
                case 16:
                    node.ResizePreset = ResizePreset.AspectCorrectedInputSizeDownward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var graphNode in GraphNodes)
            {
                CalculateResize(graphNode, graphNode == node);
            }

            GraphPaintPanel.Invalidate();
        }

        private void DownscaleAlgorithmDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            switch (DownscaleAlgorithmDropDownList.SelectedIndex)
            {
                case 0:
                    node.ResizeAlgorithm = ResizeAlgorithm.None;
                    break;
                case 1:
                    node.ResizeAlgorithm = ResizeAlgorithm.Spline64;
                    break;
                case 2:
                    node.ResizeAlgorithm = ResizeAlgorithm.Lanczos;
                    break;
                case 3:
                    node.ResizeAlgorithm = ResizeAlgorithm.Bicubic;
                    break;
                case 4:
                    node.ResizeAlgorithm = ResizeAlgorithm.Bilinear;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CalculateResize(node, true);

            GraphPaintPanel.Invalidate();
        }

        public void CalculateResize(GraphNode node, bool updateUi = false)
        {
            if (node.Inputs.Count == 0)
            {
                node.Resize.Width = 1;
                node.Resize.Height = 1;
                if (updateUi)
                {
                    WidthUpDown.Value = 1;
                    HeightUpDown.Value = 1;
                }
                return;
            }

            if (node.Inputs.Count == 1)
            {
                switch (node.ResizePreset)
                {
                    case ResizePreset.InputSize:
                        node.Resize = new Size(node.Inputs[0].Source.OutputSize);
                        break;
                    case ResizePreset.SourceSize:
                        node.Resize = new Size(Project.InputSize);
                        break;
                    case ResizePreset.AspectCorrectedSourceSizeUpward:
                        node.Resize = new Size(Project.InputSize.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Width * 3 / 4 : Project.InputSize.Width * 9 / 16));
                        if (node.Resize.Height < Project.InputSize.Height)
                        {
                            node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Height * 4 / 3 : Project.InputSize.Height * 16 / 9), Project.InputSize.Height);
                        }
                        break;
                    case ResizePreset.AspectCorrectedSourceSizeDownward:
                        node.Resize = new Size(Project.InputSize.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Width * 3 / 4 : Project.InputSize.Width * 9 / 16));
                        if (node.Resize.Height > Project.InputSize.Height)
                        {
                            node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Height * 4 / 3 : Project.InputSize.Height * 16 / 9), Project.InputSize.Height);
                        }
                        break;
                    case ResizePreset.FixedSize:
                        break;
                    case ResizePreset.FinalSize:
                        node.Resize = new Size(Project.OutputSize);
                        break;
                    case ResizePreset.TwoThirdsFinalSize:
                        node.Resize = new Size(Project.OutputSize);
                        node.Resize *= 2;
                        node.Resize /= 3;
                        break;
                    case ResizePreset.HalfFinalSize:
                        node.Resize = new Size(Project.OutputSize);
                        node.Resize /= 2;
                        break;
                    case ResizePreset.QuarterFinalSize:
                        node.Resize = new Size(Project.OutputSize);
                        node.Resize /= 4;
                        break;
                    case ResizePreset.TwoThirdsFinalSizeLimitBySource:
                        node.Resize = new Size(Project.OutputSize);
                        node.Resize *= 2;
                        node.Resize /= 3;
                        if (Project.InputSize != null && Project.InputSize.Width > node.Resize.Width)
                        {
                            node.Resize.Width = Project.InputSize.Width;
                        }
                        if (Project.InputSize != null && Project.InputSize.Height > node.Resize.Height)
                        {
                            node.Resize.Height = Project.InputSize.Height;
                        }
                        break;
                    case ResizePreset.HalfFinalSizeLimitBySource:
                        node.Resize = new Size(Project.OutputSize);
                        node.Resize /= 2;
                        if (Project.InputSize != null && Project.InputSize.Width > node.Resize.Width)
                        {
                            node.Resize.Width = Project.InputSize.Width;
                        }
                        if (Project.InputSize != null && Project.InputSize.Height > node.Resize.Height)
                        {
                            node.Resize.Height = Project.InputSize.Height;
                        }
                        break;
                    case ResizePreset.QuarterFinalSizeLimitBySource:
                        node.Resize = new Size(Project.OutputSize);
                        node.Resize /= 4;
                        if (Project.InputSize != null && Project.InputSize.Width > node.Resize.Width)
                        {
                            node.Resize.Width = Project.InputSize.Width;
                        }
                        if (Project.InputSize != null && Project.InputSize.Height > node.Resize.Height)
                        {
                            node.Resize.Height = Project.InputSize.Height;
                        }
                        break;
                    case ResizePreset.TwoThirdsInputSize:
                        node.Resize = new Size(node.Inputs[0].Source.OutputSize);
                        node.Resize *= 2;
                        node.Resize /= 3;
                        break;
                    case ResizePreset.HalfInputSize:
                        node.Resize = new Size(node.Inputs[0].Source.OutputSize);
                        node.Resize /= 2;
                        break;
                    case ResizePreset.QuarterInputSize:
                        node.Resize = new Size(node.Inputs[0].Source.OutputSize);
                        node.Resize /= 4;
                        break;
                    case ResizePreset.AspectCorrectedInputSizeUpward:
                        var inputSize = node.Inputs[0].Source.OutputSize;
                        node.Resize = new Size(inputSize.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize.Width * 3 / 4 : inputSize.Width * 9 / 16));
                        if (node.Resize.Height < inputSize.Height)
                        {
                            node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize.Height * 4 / 3 : inputSize.Height * 16 / 9), inputSize.Height);
                        }
                        break;
                    case ResizePreset.AspectCorrectedInputSizeDownward:
                        var inputSize2 = node.Inputs[0].Source.OutputSize;
                        node.Resize = new Size(inputSize2.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize2.Width * 3 / 4 : inputSize2.Width * 9 / 16));
                        if (node.Resize.Height > inputSize2.Height)
                        {
                            node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize2.Height * 4 / 3 : inputSize2.Height * 16 / 9), inputSize2.Height);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (node.Resize.Width < 1)
                {
                    node.Resize.Width = 1;
                }
                if (node.Resize.Height < 1)
                {
                    node.Resize.Height = 1;
                }

                if (updateUi)
                {
                    WidthUpDown.Value = node.Resize.Width;
                    HeightUpDown.Value = node.Resize.Height;
                }
                return;
            }

            switch (node.ResizePreset)
            {
                case ResizePreset.InputSize:
                    node.Resize = new Size(node.Inputs.Max(i => i.Source.OutputSize.Width), node.Inputs.Max(i => i.Source.OutputSize.Height));
                    break;
                case ResizePreset.SourceSize:
                    node.Resize = new Size(Project.InputSize);
                    break;
                case ResizePreset.AspectCorrectedSourceSizeUpward:
                    node.Resize = new Size(Project.InputSize.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Width * 3 / 4 : Project.InputSize.Width * 9 / 16));
                    if (node.Resize.Height < Project.InputSize.Height)
                    {
                        node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Height * 4 / 3 : Project.InputSize.Height * 16 / 9), Project.InputSize.Height);
                    }
                    break;
                case ResizePreset.AspectCorrectedSourceSizeDownward:
                    node.Resize = new Size(Project.InputSize.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Width * 3 / 4 : Project.InputSize.Width * 9 / 16));
                    if (node.Resize.Height > Project.InputSize.Height)
                    {
                        node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? Project.InputSize.Height * 4 / 3 : Project.InputSize.Height * 16 / 9), Project.InputSize.Height);
                    }
                    break;
                case ResizePreset.FixedSize:
                    break;
                case ResizePreset.FinalSize:
                    node.Resize = new Size(Project.OutputSize);
                    break;
                case ResizePreset.TwoThirdsFinalSize:
                    node.Resize = new Size(Project.OutputSize);
                    node.Resize *= 2;
                    node.Resize /= 3;
                    break;
                case ResizePreset.HalfFinalSize:
                    node.Resize = new Size(Project.OutputSize);
                    node.Resize /= 2;
                    break;
                case ResizePreset.QuarterFinalSize:
                    node.Resize = new Size(Project.OutputSize);
                    node.Resize /= 4;
                    break;
                case ResizePreset.TwoThirdsFinalSizeLimitBySource:
                    node.Resize = new Size(Project.OutputSize);
                    node.Resize *= 2;
                    node.Resize /= 3;
                    if (Project.InputSize != null && Project.InputSize.Width > node.Resize.Width)
                    {
                        node.Resize.Width = Project.InputSize.Width;
                    }
                    if (Project.InputSize != null && Project.InputSize.Height > node.Resize.Height)
                    {
                        node.Resize.Height = Project.InputSize.Height;
                    }
                    break;
                case ResizePreset.HalfFinalSizeLimitBySource:
                    node.Resize = new Size(Project.OutputSize);
                    node.Resize /= 2;
                    if (Project.InputSize != null && Project.InputSize.Width > node.Resize.Width)
                    {
                        node.Resize.Width = Project.InputSize.Width;
                    }
                    if (Project.InputSize != null && Project.InputSize.Height > node.Resize.Height)
                    {
                        node.Resize.Height = Project.InputSize.Height;
                    }
                    break;
                case ResizePreset.QuarterFinalSizeLimitBySource:
                    node.Resize = new Size(Project.OutputSize);
                    node.Resize /= 4;
                    if (Project.InputSize != null && Project.InputSize.Width > node.Resize.Width)
                    {
                        node.Resize.Width = Project.InputSize.Width;
                    }
                    if (Project.InputSize != null && Project.InputSize.Height > node.Resize.Height)
                    {
                        node.Resize.Height = Project.InputSize.Height;
                    }
                    break;
                case ResizePreset.TwoThirdsInputSize:
                    node.Resize = new Size(node.Inputs.Max(i => i.Source.OutputSize.Width), node.Inputs.Max(i => i.Source.OutputSize.Height));
                    node.Resize *= 2;
                    node.Resize /= 3;
                    break;
                case ResizePreset.HalfInputSize:
                    node.Resize = new Size(node.Inputs.Max(i => i.Source.OutputSize.Width), node.Inputs.Max(i => i.Source.OutputSize.Height));
                    node.Resize /= 2;
                    break;
                case ResizePreset.QuarterInputSize:
                    node.Resize = new Size(node.Inputs.Max(i => i.Source.OutputSize.Width), node.Inputs.Max(i => i.Source.OutputSize.Height));
                    node.Resize /= 4;
                    break;
                case ResizePreset.AspectCorrectedInputSizeUpward:
                    var inputSize = new Size(node.Inputs.Max(i => i.Source.OutputSize.Width), node.Inputs.Max(i => i.Source.OutputSize.Height));
                    node.Resize = new Size(inputSize.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize.Width * 3 / 4 : inputSize.Width * 9 / 16));
                    if (node.Resize.Height < inputSize.Height)
                    {
                        node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize.Height * 4 / 3 : inputSize.Height * 16 / 9), inputSize.Height);
                    }
                    break;
                case ResizePreset.AspectCorrectedInputSizeDownward:
                    var inputSize2 = new Size(node.Inputs.Max(i => i.Source.OutputSize.Width), node.Inputs.Max(i => i.Source.OutputSize.Height));
                    node.Resize = new Size(inputSize2.Width, (Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize2.Width * 3 / 4 : inputSize2.Width * 9 / 16));
                    if (node.Resize.Height > inputSize2.Height)
                    {
                        node.Resize = new Size((Project.AspectRatio == AspectRatio.Ratio4by3 ? inputSize2.Height * 4 / 3 : inputSize2.Height * 16 / 9), inputSize2.Height);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (node.Resize.Width < 1)
            {
                node.Resize.Width = 1;
            }
            if (node.Resize.Height < 1)
            {
                node.Resize.Height = 1;
            }

            if (updateUi)
            {
                WidthUpDown.Value = node.Resize.Width;
                HeightUpDown.Value = node.Resize.Height;
            }
        }

        private void NoiseUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            node.Noise = (float)NoiseUpDown.Value;

            GraphPaintPanel.Invalidate();
        }

        private void NoisePresetDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            switch (NoisePresetDropDownList.SelectedIndex)
            {
                case 0:
                    node.NoisePreset = NoisePreset.QtgmcPlacebo;
                    break;
                case 1:
                    node.NoisePreset = NoisePreset.QtgmcVerySlow;
                    break;
                case 2:
                    node.NoisePreset = NoisePreset.QtgmcSlower;
                    break;
                case 3:
                    node.NoisePreset = NoisePreset.QtgmcSlower;
                    break;
                case 4:
                    node.NoisePreset = NoisePreset.QtgmcMedium;
                    break;
                case 5:
                    node.NoisePreset = NoisePreset.QtgmcFast;
                    break;
                case 6:
                    node.NoisePreset = NoisePreset.QtgmcFaster;
                    break;
                case 7:
                    node.NoisePreset = NoisePreset.QtgmcVeryFast;
                    break;
                case 8:
                    node.NoisePreset = NoisePreset.QtgmcSuperFast;
                    break;
                case 9:
                    node.NoisePreset = NoisePreset.QtgmcUltraFast;
                    break;
                case 10:
                    node.NoisePreset = NoisePreset.QtgmcDraft;
                    break;
                case 11:
                    node.NoisePreset = NoisePreset.FilmGrain005;
                    break;
                case 12:
                    node.NoisePreset = NoisePreset.FilmGrain010;
                    break;
                case 13:
                    node.NoisePreset = NoisePreset.FilmGrain015;
                    break;
                case 14:
                    node.NoisePreset = NoisePreset.FilmGrain020;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GraphPaintPanel.Invalidate();
        }

        private void WidthUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            node.Resize.Width = (int)WidthUpDown.Value;
            OutputResolutionTextBox.Text = node.OutputSize.ToString();

            GraphPaintPanel.Invalidate();
        }

        private void HeightUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            node.Resize.Height = (int)HeightUpDown.Value;
            OutputResolutionTextBox.Text = node.OutputSize.ToString();

            GraphPaintPanel.Invalidate();
        }

        private void WidthHeightUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            PresetDropDownList.SelectedIndex = 4; // Fixed size
        }

        private void AutoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            RevertCompressionUpDown.Enabled = !AutoCheckBox.Checked;
            RecoverDetailsUpDown.Enabled = !AutoCheckBox.Checked;
            SharpenUpDown.Enabled = !AutoCheckBox.Checked;
            ReduceNoiseUpDown.Enabled = !AutoCheckBox.Checked;
            DehaloUpDown.Enabled = !AutoCheckBox.Checked;
            AntiAliasDeblurUpDown.Enabled = !AutoCheckBox.Checked;

            if (isUpdatingComponentSettings)
            {
                return;
            }

            node.Auto = AutoCheckBox.Checked;

            GraphPaintPanel.Invalidate();
        }

        private void ProteusSettingUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var node = SelectedNode;
            if (node == null)
            {
                return;
            }

            node.RevertCompression = (int)RevertCompressionUpDown.Value;
            node.RecoverDetails = (int)RecoverDetailsUpDown.Value;
            node.Sharpen = (int)SharpenUpDown.Value;
            node.ReduceNoise = (int)ReduceNoiseUpDown.Value;
            node.Dehalo = (int)DehaloUpDown.Value;
            node.AntiAliasDeblur = (int)AntiAliasDeblurUpDown.Value;
            node.RecoverOriginalDetails = (int)RecoverOriginalDetailsUpDown.Value;
            node.OffsetX = (int)(OffsetXUpDown.Value * 4);
            node.OffsetY = (int)(OffsetYUpDown.Value * 4);

            GraphPaintPanel.Invalidate();
        }

        private void WeightUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingComponentSettings)
            {
                return;
            }

            var input = SelectedInput;
            if (input == null)
            {
                return;
            }

            input.Weight = (float)WeightUpDown.Value;

            GraphPaintPanel.Invalidate();
        }




        private void FullScrubTrackBar_Scroll(object sender, EventArgs e)
        {
            PreviewScrubTrackBar.Value = 0;

            PreviewForm.Display(Project.SourceVideo);
            PreviewForm.Seek(0);
        }

        public void UpdatePreview()
        {
            var projectFolder = Path.GetDirectoryName(Project.ProjectFileName);
            var outputFolder = Path.Combine(projectFolder, "Output");

            var previewFiles = GraphNodes.Where(n => n.IsSelected).OrderBy(n => n.Left).Select(n =>
                n.IsSource
                    ? (File: Project.SourceVideo, Node: n)
                    : (File: Path.Combine(outputFolder, $"{PreviewFrame}_{PreviewLength}_{n.HashName}.avi"), Node: n)
            ).ToArray();

            for (int i = 0; i < previewFiles.Length; i++)
            {
                if (!previewFiles[i].Node.IsSource && !File.Exists(previewFiles[i].File))
                {
                    var nodeRenderer = new NodeRenderer(this, previewFiles[i].Node);
                    nodeRenderer.Render();
                }
            }

            PreviewForm.Display(previewFiles.Select(f => f.File).ToArray());
            PreviewForm.Seek(PreviewScrubTrackBar.Value);
        }

        private void PreviewScrubTrackBar_Scroll(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void RenderLengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewScrubTrackBar.Value = 0;
            switch (RenderLengthComboBox.SelectedIndex)
            {
                case 0:
                    PreviewScrubTrackBar.Maximum = FullScrubTrackBar.Maximum;
                    break;
                case 1:
                    PreviewScrubTrackBar.Maximum = 4 - 1;
                    break;
                case 2:
                    PreviewScrubTrackBar.Maximum = 10 - 1;
                    break;
                case 3:
                    PreviewScrubTrackBar.Maximum = (int)Math.Round(Project.Framerate * 1) - 1;
                    break;
                case 4:
                    PreviewScrubTrackBar.Maximum = (int)Math.Round(Project.Framerate * 5) - 1;
                    break;
                case 5:
                    PreviewScrubTrackBar.Maximum = (int)Math.Round(Project.Framerate * 30) - 1;
                    break;
            }

            //PreviewScrubTrackBar.Maximum = 
        }

        private void RenderLengthComboBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(RenderLengthComboBox.Text, out int length))
            {
                PreviewScrubTrackBar.Maximum = 0;
                PreviewScrubTrackBar.Value = 0;
                PreviewScrubTrackBar.Maximum = length - 1;
            }
        }

        private void RenderButton_Click(object sender, EventArgs e)
        {
            GraphNode[] renderNodes;

            bool areNodesSelected = GraphNodes.Any(n => n.IsSelected && !n.IsSource);
            if (areNodesSelected)
            {
                renderNodes = GraphNodes.Where(n => n.IsSelected).ToArray();
            }
            else
            {
                var connectionSources = GraphNodes.SelectMany(n => n.Inputs.Select(i => i.Source)).Distinct().ToArray();
                renderNodes = GraphNodes.Where(n => !n.IsSource && !connectionSources.Contains(n)).ToArray();
            }

            foreach (var renderNode in renderNodes)
            {
                renderNode.Render();
            }

            if (areNodesSelected)
            {
                UpdatePreview();
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}

