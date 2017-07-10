using System.Runtime.InteropServices;

namespace HuntAndPeck.NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right= right;
            this.bottom = bottom;
        }

        public static implicit operator System.Windows.Rect(RECT r)
        {
            return new System.Windows.Rect(r.left, r.top, r.right - r.left, r.bottom - r.top);
        }

        public static implicit operator RECT(System.Windows.Rect r)
        {
            return new RECT((int)r.X, (int)r.Y, (int)(r.X + r.Width), (int)(r.Y + r.Height));
        }

        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);
        }

        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }
    }
}
