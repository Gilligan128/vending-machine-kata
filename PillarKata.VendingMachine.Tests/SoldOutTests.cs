using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SoldOutTests
    {
        private readonly TestBuilder _testBuilder = new TestBuilder();

        [Fact]
        public void ShouldDisplaySoldOutWhenButtonPressedButProductNotAvailable()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(ProductCodes.Chips);

            var display = sut.CheckDisplay();
            Assert.Equal("SOLD OUT", display);
        }
    }
}
