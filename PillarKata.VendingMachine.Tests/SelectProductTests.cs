using System.Security.Cryptography;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SelectProductTests
    {
        private const string ColaButton = "COLA";

        [Fact]
        public void SouldShowDefaultMessageWhenInvalidButtonIsEntered()
        {
            var sut = new VendingMachine();

            sut.PressButton("Nothing");

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display.Message);
        }

        [Theory]
        [InlineData("Cola", "1.00")]
        [InlineData("Chips", "0.50")]
        [InlineData("Candy", "0.65")]
        public void ShouldShowInsufficientCoinsDisplayWhenButtonIspressedWithoutEnoughCoinsInserted(string buttonCode, string price)
        {
            var sut = new VendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal(string.Format("PRICE ${0}", price), display.Message);
        }

        [Fact]
        public void ShouldShowDefaultMessageWhenButtonIsPresedWithoutENoughCoinsAndDisplayIsCheckedOnce()
        {
            var sut = new VendingMachine();

            sut.PressButton(ColaButton);
            sut.CheckDisplay();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display.Message);
            //Assert.Equal(0m, display.Amount);
        }
    }
}
