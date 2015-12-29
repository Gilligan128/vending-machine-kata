using System.Collections.Generic;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SoldOutTests
    {
        private readonly TestBuilder _testBuilder = new TestBuilder();

        [Fact]
        public void ShouldDisplaySoldOutWhenCoinsInsertedAndButtonPressedButProductNotAvailable()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(ProductCodes.Chips);

            var display = sut.CheckDisplay();
            Assert.Equal("SOLD OUT", display);
        }

        [Fact]
        public void ShouldDisplaySoldOutWhenButtonPressedButProductNotAvailable()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            sut.PressButton(ProductCodes.Chips);

            var display = sut.CheckDisplay();
            Assert.Equal("SOLD OUT", display);
        }

        [Fact]
        public void ShouldDisplayDefaultMessageWhenUnavailablProductButtonPressedAndDisplayChecked()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            sut.StockCoins(new Dictionary<double, int> { { CoinWeights.Quarter, 10 } });
            sut.PressButton(ProductCodes.Chips);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

    }
}
