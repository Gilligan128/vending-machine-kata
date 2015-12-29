using System.Security.Cryptography;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class SelectProductTests
    {

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
        public void ShouldShowInsufficientCoinsDisplayWhenButtonIspressedWithoutEnoughCoinsInserted(string buttonCode, string price)
        {
            var sut = new VendingMachine();

            sut.PressButton(buttonCode);

            var display = sut.CheckDisplay();
            Assert.Equal(string.Format("PRICE ${0}", price), display.Message);
        }
    }
}
