using System.Drawing.Drawing2D;
using VideoProcessingLib.AviSynth;

namespace TopazVideoLab2
{
    public partial class PreviewForm : Form
    {
        public MainForm MainForm { get; }
        private string[] Files { get; set; }
        private AviReader[] AviReader { get; set; }

        private GraphNode[] Nodes { get; set; }

        private int CurrentImage { get; set; } = 0;

        private int OffsetX { get; set; }
        private int OffsetY { get; set; }
        private int Zoom { get; set; } = 1;

        public PreviewForm(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();
        }

        public void Display(params string[] files)
        {
            if (Files != null && Files.Length == files.Length && files.Zip(Files).All(f => f.First == f.Second))
            {
                return;
            }
            Files = files;

            if (AviReader != null)
            {
                foreach (var aviReader in AviReader)
                {
                    aviReader.Dispose();
                }
            }

            AviReader = new AviReader[Files.Length];
            for (int i = 0; i < Files.Length; i++)
            {
                AviReader[i] = new AviReader(Files[i], false, ColorSpace.RGB, true);
            }

            Nodes = new GraphNode[Files.Length];
            for (int i = 0; i < Files.Length; i++)
            {
                if (Files[i] == MainForm.Project.SourceVideo)
                {
                    Nodes[i] = MainForm.GraphNodes.FirstOrDefault(n => n.IsSource);
                    continue;
                }
                Nodes[i] = MainForm.GraphNodes.First(n => Files[i].EndsWith($"_{n.HashName}.avi"));
            }

            int maxWidth = 0;
            int maxHeight = 0;
            for (int i = 0; i < Files.Length; i++)
            {
                int width;
                int height;
                if (Files[i] == MainForm.Project.SourceVideo)
                {
                    width = MainForm.Project.InputSize.Width;
                    height = MainForm.Project.InputSize.Height;
                }
                else
                {
                    width = Nodes[i].OutputSize.Width;
                    height = Nodes[i].OutputSize.Height;
                }

                if (width > maxWidth)
                {
                    maxWidth = width;
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                }
            }

            for (int i = 0; i < Files.Length; i++)
            {
                int width;
                int height;
                if (Files[i] == MainForm.Project.SourceVideo)
                {
                    width = MainForm.Project.InputSize.Width;
                    height = MainForm.Project.InputSize.Height;
                }
                else
                {
                    width = Nodes[i].OutputSize.Width;
                    height = Nodes[i].OutputSize.Height;
                }

                if (width != maxWidth || height != maxHeight)
                {
                    AviReader[i].ResizeMode = VideoProcessingLib.AviSynth.AviReader.ResizeFilter.Bilinear;
                    AviReader[i].Resize = new VideoProcessingLib.AviSynth.Size(maxWidth, maxHeight);
                    AviReader[i].Reload();
                }
            }

            CurrentImage = 0;

            HighlightCurrentNode();
        }

        public void Seek(int frame)
        {
            for (var i = 0; i < Files.Length; i++)
            {
                if (Files[i] == MainForm.Project.SourceVideo)
                {
                    AviReader[i].Position = frame + MainForm.PreviewFrame;
                }
                else
                {
                    AviReader[i].Position = frame;
                }
            }

            PaintPanel.Invalidate();
        }


        private void PaintPanel_PaintDoubleBuffer(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.Bilinear;

            using (var black = new SolidBrush(Color.Black))
            {
                e.Graphics.FillRectangle(black, e.ClipRectangle);
            }

            if (AviReader == null)
            {
                return;
            }

            e.Graphics.DrawImage(AviReader[CurrentImage].Image, OffsetX + MouseDeltaX, OffsetY + MouseDeltaY, AviReader[CurrentImage].Image.Width * Zoom, AviReader[CurrentImage].Image.Height * Zoom);
        }

        private bool IsMouseDown = false;
        private int MouseStartDragX;
        private int MouseStartDragY;
        private int MouseDeltaX;
        private int MouseDeltaY;

        private void PaintPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (Capture) {}

            IsMouseDown = true;
            MouseStartDragX = e.Location.X;
            MouseStartDragY = e.Location.Y;

            PaintPanel.Focus();
        }

        private void PaintPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                OffsetX += MouseDeltaX;
                OffsetY += MouseDeltaY;
                MouseDeltaX = 0;
                MouseDeltaY = 0;

                PaintPanel.Invalidate();
            }
            IsMouseDown = false;
        }

        private void PaintPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                MouseDeltaX = e.Location.X - MouseStartDragX;
                MouseDeltaY = e.Location.Y - MouseStartDragY;

                PaintPanel.Invalidate();
            }
            PaintPanel.Focus();
        }

        private void PaintPanel_MouseWheelScroll(object sender, MouseEventArgs e)
        {
            int oldZoom = Zoom;
            if (e.Delta > 0 && Zoom < 8)
            {
                Zoom *= 2;
            } else if (e.Delta < 0 && Zoom > 1)
            {
                Zoom /= 2;
            }

            if (Zoom == oldZoom)
            {
                return;
            }

            /*int leftOffset = OffsetX - e.Location.X;
            int topOffset = OffsetY - e.Location.Y;

            if (Zoom > oldZoom)
            {
                leftOffset *= 2;
                topOffset *= 2;
            }
            else
            {
                leftOffset /= 2;
                topOffset /= 2;
            }

            OffsetX = e.Location.X + leftOffset;
            OffsetY = e.Location.Y + topOffset;*/


            OffsetX = e.Location.X + (OffsetX - e.Location.X) * Zoom / oldZoom;
            OffsetY = e.Location.Y + (OffsetY - e.Location.Y) * Zoom / oldZoom;


            PaintPanel.Invalidate();
        }

        private void PaintPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (MainForm.Project.ProjectFileName == null)
            {
                MainForm.SaveAsMenuItem_Click(null, null);
            }

            if (MainForm.Project.ProjectFileName == null)
            {
                MessageBox.Show(this, "Project must be saved for previews to work.", "Project not saved", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var projectFolder = Path.GetDirectoryName(MainForm.Project.ProjectFileName);
            var outputFolder = Path.Combine(projectFolder, "Output");

            var previewFiles = MainForm.GraphNodes.Where(n => n.IsSelected).OrderBy(n => n.Left).Select(n =>
                n.IsSource
                    ? MainForm.Project.SourceVideo
                    : Path.Combine(outputFolder, $"{MainForm.PreviewFrame}_{MainForm.PreviewLength}_{n.HashName}.avi")
            ).ToArray();

            if (Files == null || Files.Length != previewFiles.Length || previewFiles.Zip(Files).Any(f => f.First != f.Second))
            {
                MainForm.UpdatePreview();
            }

            int currentImage = CurrentImage;
            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                case Keys.D1:
                    currentImage = 0;
                    break;
                case Keys.NumPad2:
                case Keys.D2:
                    currentImage = 1;
                    break;
                case Keys.NumPad3:
                case Keys.D3:
                    currentImage = 2;
                    break;
                case Keys.NumPad4:
                case Keys.D4:
                    currentImage = 3;
                    break;
                case Keys.NumPad5:
                case Keys.D5:
                    currentImage = 4;
                    break;
                case Keys.NumPad6:
                case Keys.D6:
                    currentImage = 5;
                    break;
                case Keys.NumPad7:
                case Keys.D7:
                    currentImage = 6;
                    break;
                case Keys.NumPad8:
                case Keys.D8:
                    currentImage = 7;
                    break;
                case Keys.NumPad9:
                case Keys.D9:
                    currentImage = 8;
                    break;
                case Keys.NumPad0:
                case Keys.D0:
                    currentImage = 9;
                    break;
            }

            if (currentImage < Files.Length)
            {
                CurrentImage = currentImage;
                PaintPanel.Invalidate();

                HighlightCurrentNode();
            }
        }

        private void HighlightCurrentNode()
        {
            for (int i = 0; i < Files.Length; i++)
            {
                if (Nodes[i] == null)
                {
                    continue;
                }

                if (i == CurrentImage)
                {
                    Nodes[i].IsHighlighted = true;
                }
                else
                {
                    Nodes[i].IsHighlighted = false;
                }
            }

            MainForm.GraphPaintPanel.Invalidate();
        }
    }
}


