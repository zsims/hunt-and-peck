using hap.Models;

namespace hap.ViewModels
{
    public class DebugHintViewModel
    {
        public DebugHintViewModel(DebugHint hint)
        {
            Hint = hint;
            Text = string.Join(", ", hint.SupportedPatterns);

            // make the text short so it fits in the overlay. "PatternIdentifiers.Pattern" is in every id
            var vowels = new[] {"a", "e", "i", "o", "u"};
            ShortText = Text.Replace("PatternIdentifiers.Pattern", string.Empty);
            foreach(var vowel in vowels)
            {
                ShortText = ShortText.Replace(vowel, string.Empty);
            }
        }

        public DebugHint Hint { get; set; }
        public string Text { get; set; }
        public string ShortText { get; set; }
    }
}
