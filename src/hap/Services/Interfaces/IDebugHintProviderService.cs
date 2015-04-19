using System;
using hap.Models;

namespace hap.Services.Interfaces
{
    public interface IDebugHintProviderService
    {
        HintSession EnumDebugHints();
        HintSession EnumDebugHints(IntPtr hWnd);
    }
}
