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
    }
}
