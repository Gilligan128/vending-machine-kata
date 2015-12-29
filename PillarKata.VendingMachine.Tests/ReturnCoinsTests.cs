using System.Linq;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class ReturnCoinsTests
    {
        private TestBuilder _testBuilder = new TestBuilder();

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 0)]
        [InlineData(0,0,0)]
        public void SouldReturnCoinsInsertedWhenReturnCoinsIsPressed(int numberOfQuarters, int numberOfDimes, int numberOfNickels)
        {
            var sut = _testBuilder.CreateVendingMachine();

            for (var i = 0; i < numberOfQuarters; i++) 
                sut.InsertCoin(_testBuilder.CreateQuarter());
            for (var i = 0; i < numberOfDimes; i++) 
                sut.InsertCoin(_testBuilder.CreateDime());
            for(var i = 0; i < numberOfNickels; i++)
                sut.InsertCoin(_testBuilder.CreateNickel());
            sut.ReturnCoins();

            var coinReturn = sut.GetCoinReturn();
            Assertions.CoinsReturned("Return Coins", numberOfQuarters, numberOfDimes, numberOfNickels, coinReturn);

        }
    }
}
