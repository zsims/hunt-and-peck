using hap.Models;
using System;

namespace hap.Services.Interfaces
{
    public interface ISessionCache
    {
        void SetSession(IntPtr hWnd, HintSession session);
        HintSession GetSession(IntPtr hWnd);
    }
}
