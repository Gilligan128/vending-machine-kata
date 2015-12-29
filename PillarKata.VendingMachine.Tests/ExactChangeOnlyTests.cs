using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class ExactChangeOnlyTests
    {
        [Fact]
        public void ShouldDisplayExactChangeOnlyWhenThereArenoCoinsStocked()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            var display = sut.CheckDisplay();
            Assert.Equal("EXACT CHANGE ONLY", display);
        }
    }
}
