
namespace hap.NativeMethods
{
    /// <summary>
    /// Event constants per https://msdn.microsoft.com/en-us/library/windows/desktop/dd318066%28v=vs.85%29.aspx
    /// </summary>
    public static class EventConstants
    {
        /// <summary>
        /// The foreground window has changed. The system sends this event even if the foreground window has changed to another window in the same thread. Server applications never send this event.
        /// </summary>
        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
    }
}
