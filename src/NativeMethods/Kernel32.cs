using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HuntAndPeck.NativeMethods
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
