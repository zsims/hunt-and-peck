using System.Windows.Automation;

namespace hap.Engine.Hints
{
    internal class UiAutomationInvokeCapability : HintCapabilityBase
    {
        private readonly InvokePattern _invokePattern;

        public UiAutomationInvokeCapability(InvokePattern invokePattern)
        {
            _invokePattern = invokePattern;
        }

        public override HintCapabilityIdentifer Identifier
        {
            get
            {
                return HintCapabilityIdentifer.Invoke;
            }
        }

        public override void Activate()
        {
            _invokePattern.Invoke();
        }
    }
}
