using System;
using HuntAndPeck.Models;

namespace HuntAndPeck.Services.Interfaces
{
    public interface IDebugHintProviderService
    {
        HintSession EnumDebugHints();
        HintSession EnumDebugHints(IntPtr hWnd);
    }
}
