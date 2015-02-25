using System.Collections.Generic;

namespace hap.Services.Interfaces
{
    public interface IHintLabelService
    {
        /// <summary>
        /// Generate N labels
        /// </summary>
        IList<string> GetHintStrings(int count);
    }
}
