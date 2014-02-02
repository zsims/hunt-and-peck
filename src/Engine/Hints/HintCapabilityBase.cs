
namespace HuntAndPeck.Engine.Hints
{
    /// <summary>
    /// Represents the capabilities of a given hint
    /// </summary>
    public abstract class HintCapabilityBase
    {
        /// <summary>
        /// Invokes the hint capability
        /// </summary>
        public abstract void Activate();

        /// <summary>
        /// The identifier of the hint capability
        /// </summary>
        public abstract HintCapabilityIdentifer Identifier { get; }
    }
}
