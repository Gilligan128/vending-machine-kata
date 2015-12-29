using Microsoft.SqlServer.Server;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SelectProductTests
    {
        private readonly TestBuilder _testBuilder = new TestBuilder();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Nothing")]
        public void ShouldShowDefaultMessageWhenInvalidButtonIsEntered(string buttonCode)
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Fact]
        public void ShouldNotDispenseProductWhenInvalidButtonIsEntered()
        {
            var dispenser = new StubbedDispenser();
            var sut = new VendingMachine(dispenser);

            sut.PressButton("Invalid");

            Assert.False(dispenser.WasProductDispensed);
        }

        [Theory]
        [InlineData(ProductCodes.Cola, "1.00")]
        [InlineData(ProductCodes.Chips, "0.50")]
        [InlineData(ProductCodes.Candy, "0.65")]
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

            sut.PressButton(ProductCodes.Cola);

            Assert.False(dispenser.WasProductDispensed);
        }

        [Fact]
        public void ShouldShowDefaultMessageWhenButtonIsPressedWithoutEnoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.PressButton(ProductCodes.Cola);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Fact]
        public void ShouldShowAmountWhenButtonIsPressedAndCoinsInsertedWithoutEnoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(new Coin(CoinWeights.Nickel));
            sut.PressButton(ProductCodes.Cola);
            sut.CheckDisplay(); //The thought did cross my mind that we may be breaking Command/Query Separation here

            var display = sut.CheckDisplay();
            Assert.Equal("$0.05", display);
        }

        [Theory]
        [InlineData(ProductCodes.Cola, 4)]
        [InlineData(ProductCodes.Chips, 2)]
        [InlineData(ProductCodes.Candy, 3)]
        public void ShouldDispenseProductWhenButtonIsPressedWithEnoughCoinsInserted(string productCode, int numberOfQuartersInserted)
        {
            var dispenser = new StubbedDispenser();
            var sut = new VendingMachine(dispenser);

            for (var i = 0; i < numberOfQuartersInserted; i++)
            {
               sut.InsertCoin(_testBuilder.CreateQuarter()); 
            }
            sut.PressButton(productCode);

            Assert.True(dispenser.WasProductDispensed);
        }

        [Theory]
        [InlineData(ProductCodes.Cola, 4)]
        [InlineData(ProductCodes.Chips, 2)]
        [InlineData(ProductCodes.Candy, 3)]
        public void ShouldDisplayThankYouWhenButtonIsPressedWithEnoughCoinsInserted(string productCode, int numberOfQuartersInserted)
        {
            var dispenser = new StubbedDispenser();
            var sut = new VendingMachine(dispenser);

            for (var i = 0; i < numberOfQuartersInserted; i++)
            {
                sut.InsertCoin(_testBuilder.CreateQuarter());
            }
            sut.PressButton(productCode);

            var display = sut.CheckDisplay();
            Assert.Equal("THANK YOU", display);
        }

        [Fact]
        public void ShouldDisplayDefaultMessageWhenButtonIsPressedWithEnoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(ProductCodes.Chips);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }
    }
}