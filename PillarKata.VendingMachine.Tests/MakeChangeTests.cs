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

        [Fact]
        public void ShouldMakeChangeWhenMoreCoinsInsertedThanProductPrice()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.PressButton(ProductCodes.Cola);

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(1, coinReturn.Count);
            Assert.Equal(CoinWeights.Quarter, coinReturn.First().WeightInGrams);
        }

        
    }
}
