namespace Cybercraft.Common.WinForms
{
    public class Uv
    {
        public int U;
        public int V;

        public override string ToString()
        {
            return $"{U},{V}";
        }

        public Uv(int u, int v)
        {
            U = u;
            V = v;
        }

        public Uv(Uv uv)
        {
            if (uv != null)
            {
                U = uv.U;
                V = uv.V;
            }
        }
    }
}