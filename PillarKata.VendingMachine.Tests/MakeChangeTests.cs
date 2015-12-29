using System.Globalization;
using System.Linq;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class MakeChangeTests
    {

        TestBuilder _testBuilder = new TestBuilder();

        [Fact]
        public void ShouldNotMakeChangeWhenExactChangeIsInsertedAndProductSelected()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(ProductCodes.Cola);

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(0, coinReturn.Count);
        }

        [Theory]
        [InlineData(ProductCodes.Cola, 1, 0, 0)]
        [InlineData(ProductCodes.Chips, 3, 0, 0)]
        [InlineData(ProductCodes.Candy, 2, 1, 0)]
        public void ShouldMakeChangeWhenMoreCoinsInsertedThanProductPrice(string productCode, int quartersReturned, int dimesReturned, int nickelsReturned)
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.StockCoins(0, dimesReturned, nickelsReturned);
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(productCode);

            var coinReturn = sut.GetCoinReturn();
            var coinsReturned = quartersReturned + dimesReturned + nickelsReturned;
            Assert.True(coinsReturned == coinReturn.Count, string.Format("Product: {0}, Expected {1} Coins, Actual {2} Coins", productCode, coinsReturned, coinReturn.Count));
            Assert.Equal(quartersReturned, coinReturn.Count(x => x.WeightInGrams == CoinWeights.Quarter));
            Assert.Equal(dimesReturned, coinReturn.Count(x => x.WeightInGrams == CoinWeights.Dime));
            Assert.Equal(nickelsReturned, coinReturn.Count(x => x.WeightInGrams == CoinWeights.Nickel));
        }


    }
}
