using HuntAndPeck.Services;
using Xunit;
using System.Linq;

namespace HuntAndPeck.Tests.Services
{
    public class HintLabelServiceTest
    {
        [Fact]
        public void GetHintStrings_UniqueStrings()
        {
            // Arrange
            const int hintCount = 256;
            var hintService = new HintLabelService();

            // Act
            var hints = hintService.GetHintStrings(hintCount);

            // Assert
            Assert.Equal(hintCount, hints.Distinct().Count());
        }
    }
}
