using System;
using System.Threading;

namespace HuntAndPeck
{
    public class SingleLaunchMutex : IDisposable
    {
        private readonly bool _aquiredHandle;
        private Mutex _mutex = new Mutex(true, "5B5486F3-15E3-4DD5-BF05-03C3F716483E");

        public SingleLaunchMutex()
        {
            try
            {
                _aquiredHandle = _mutex.WaitOne(TimeSpan.Zero, true);
            }
            catch (AbandonedMutexException)
            {
                // This will happen if the mutex isn't disposed properly, e.g. during a crash
                _aquiredHandle = true;
            }
        }

        public bool AlreadyRunning
        {
            get { return !_aquiredHandle; }
        }

        public void Dispose()
        {
            if (_mutex != null)
            {
                if (_aquiredHandle)
                {
                    _mutex.ReleaseMutex();
                }

                _mutex.Dispose();
                _mutex = null;
            }
        }
    }
}
