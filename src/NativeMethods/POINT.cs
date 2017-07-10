using System.Runtime.InteropServices;

namespace HuntAndPeck.NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator System.Windows.Point(POINT p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Windows.Point p)
        {
            return new POINT((int)p.X, (int)p.Y);
        }
    }
}
