using System;
using System.Runtime.InteropServices;
using Accessibility;

namespace hap.NativeMethods
{
    public static class OleAcc
    {
        [DllImport("oleacc.dll")]
        public static extern uint WindowFromAccessibleObject(IAccessible pacc, ref IntPtr phwnd);

        [DllImport("oleacc.dll")]
        public static extern int AccessibleChildren(
            IAccessible paccContainer,
            int iChildStart,
            int cChildren,
            [Out()] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] object[] rgvarChildren,
            ref int pcObtained);

        [DllImport("oleacc.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object AccessibleObjectFromWindow(int hwnd, int dwId, ref Guid riid);
    }
}
