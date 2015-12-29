using Microsoft.SqlServer.Server;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SelectProductTests
    {
        private readonly TestBuilder _testBuilder = new TestBuilder();
        private const string Cola = "COLA";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Nothing")]
        public void SouldShowDefaultMessageWhenInvalidButtonIsEntered(string buttonCode)
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Theory]
        [InlineData(Cola, "1.00")]
        [InlineData("Chips", "0.50")]
        [InlineData("Candy", "0.65")]
        [InlineData("Cola", "1.00")] //case-sensitivity test
        public void ShouldShowInsufficientCoinsDisplayWhenButtonIspressedWithoutEnoughCoinsInserted(string buttonCode,
            string price)
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal(string.Format("PRICE ${0}", price), display);
        }

        [Fact]
        public void ShouldHaveNoDispensedProductWhenButtonIsPressedWithoutEnoughCoinsInserted()
        {
            var dispenser = new StubbedDispenser();
            var sut = new VendingMachine(dispenser);

            sut.PressButton(Cola);

            Assert.False(dispenser.WasProductDispensed);
        }

        [Fact]
        public void ShouldShowDefaultMessageWhenButtonIsPressedWithoutEnoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.PressButton(Cola);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Fact]
        public void ShouldShowAmountWhenButtonIsPressedAndCoinsInsertedWithoutEnoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(new Coin(CoinWeights.Nickel));
            sut.PressButton(Cola);
            sut.CheckDisplay(); //The thought did cross my mind that we may be breaking Command/Query Separation here

            var display = sut.CheckDisplay();
            Assert.Equal("$0.05", display);
        }

        [Fact]
        public void ShouldDispenseProductWhenButtonIsPressedWithEnoughCoinsInserted()
        {
            var dispenser = new StubbedDispenser();
            var sut = new VendingMachine(dispenser);

            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(Cola);

            Assert.True(dispenser.WasProductDispensed);
        }
    }
}