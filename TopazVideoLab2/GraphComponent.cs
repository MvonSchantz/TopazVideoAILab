namespace TopazVideoLab2
{
    public abstract class GraphComponent
    {
        public MainForm MainForm { get; }

        public int Width { get; }
        public int Height { get; }
        public int Left { get; set; }
        public int Top { get; set; }

        public int Right => Left + Width - 1;
        public int Bottom => Top + Height - 1;

        public int MidX => (Left + Right) / 2;
        public int MidY => (Top + Bottom) / 2;

        public bool IsFixed { get; }

        protected GraphComponent(MainForm mainForm, int width, int height, int left, int top, bool isFixed = false)
        {
            MainForm = mainForm;
            Width = width;
            Height = height;
            Left = left;
            Top = top;
            IsFixed = isFixed;
        }

        public virtual bool IsInside(int x, int y) => x >= Left && y >= Top && x <= Right && y <= Bottom;

        public abstract void Draw(GraphDc graph);

        public virtual void OnMouseDown() {}

        public virtual void OnMouseUp() {}

        public virtual void OnMouseDoubleClick() {}
    }
}
