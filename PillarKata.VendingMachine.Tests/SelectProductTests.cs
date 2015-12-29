using System.Security.AccessControl;
using System.Security.Cryptography;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SelectProductTests
    {
        private const string ColaButton = "Cola";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Nothing")]
        public void SouldShowDefaultMessageWhenInvalidButtonIsEntered(string buttonCode)
        {
            var sut = new VendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Theory]
        [InlineData(ColaButton, "1.00")]
        [InlineData("Chips", "0.50")]
        [InlineData("Candy", "0.65")]
        public void ShouldShowInsufficientCoinsDisplayWhenButtonIspressedWithoutEnoughCoinsInserted(string buttonCode, string price)
        {
            var sut = new VendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal(string.Format("PRICE ${0}", price), display);
        }

        [Fact]
        public void ShouldShowDefaultMessageWhenButtonIsPresedWithoutENoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = new VendingMachine();

            sut.PressButton(ColaButton);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Fact]
        public void ShouldShowAmountWhenButtonIsPressedANdCOinsINsertedWIthoutEnoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = new VendingMachine();

            sut.InsertCoin(new Coin(CoinWeights.Nickel));
            sut.PressButton(ColaButton);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("$0.05", display);
        }

       
    }
}
