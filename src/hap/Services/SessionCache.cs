using System;
using hap.Models;
using hap.Services.Interfaces;

namespace hap.Services
{
    public class SessionCache : ISessionCache
    {
        private IntPtr _hWnd;
        private HintSession _session;
        private readonly object _mutex = new object();

        public void SetSession(IntPtr hWnd, HintSession session)
        {
            lock (_mutex)
            {
                _hWnd = hWnd;
                _session = session;
            }
        }

        public HintSession GetSession(IntPtr hWnd)
        {
            lock (_mutex)
            {
                return _hWnd == hWnd ? _session : null;
            }
        }
    }
}
