using System;
using System.Windows;
using hap.Models;
using hap.NativeMethods;
using hap.Services.Interfaces;

namespace hap.Services
{
    public class AheadOfTimeSessionService : IDisposable
    {
        private readonly IHintProviderService _hintProviderService;
        private IntPtr _hookId = IntPtr.Zero;
        private readonly ISessionCache _sessionCache;
        private User32.WinEventDelegate _eventDelegate;

        public AheadOfTimeSessionService(
            IHintProviderService hintProviderService,
            ISessionCache sessionCache)
        {
            _hintProviderService = hintProviderService;
            _sessionCache = sessionCache;

            // Keep a reference to the delegate so the GC doesn't sweep it up
            _eventDelegate = EventCallback;

            _hookId = User32.SetWinEventHook(
                EventConstants.EVENT_SYSTEM_FOREGROUND, EventConstants.EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero,
                _eventDelegate,
                0,
                0,
                (uint)(User32.SetWinEventHookFlags.WINEVENT_OUTOFCONTEXT | User32.SetWinEventHookFlags.WINEVENT_SKIPOWNPROCESS));
        }

        private void EventCallback(
            IntPtr hWinEventHook,
            uint iEvent,
            IntPtr hWnd,
            int idObject,
            int idChild,
            uint dwEventThread,
            uint dwmsEventTime)
        {
            // Has to be done on the UI thread or things blow up
            // TODO: This isn't ideal as we'll lock up the UI thread obviously :)
            Application.Current.Dispatcher.Invoke(() =>
            {
                HintSession session = null;
                try
                {
                    session = _hintProviderService.EnumHints(hWnd);
                }
                catch (Exception)
                {
                    // nom nom nom
                }

                if (session != null)
                {
                    _sessionCache.SetSession(hWnd, session);
                }
            });
        }

        public void Dispose()
        {
            if (_hookId != IntPtr.Zero)
            {
                User32.UnhookWinEvent(_hookId);
                _hookId = IntPtr.Zero;
            }
        }
    }
}
