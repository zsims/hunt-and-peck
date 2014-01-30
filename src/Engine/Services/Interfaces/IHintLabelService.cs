
using HuntnPeck.Engine.Hints;
using System.Collections.Generic;

namespace HuntnPeck.Engine.Services.Interfaces
{
    public interface IHintLabelService
    {
        /// <summary>
        /// Apply unique labels to the given hints
        /// </summary>
        /// <param name="hints">The hints to label</param>
        void LabelHints(IEnumerable<Hint> hints);

        /// <summary>
        /// Returns the hints that match the given label
        /// </summary>
        /// <param name="partialLabel">The partial label</param>
        /// <param name="hints">The hints to match</param>
        /// <returns>The hints that match</returns>
        IEnumerable<Hint> FindMatchingHints(string partialLabel, IEnumerable<Hint> hints);
    }
}
