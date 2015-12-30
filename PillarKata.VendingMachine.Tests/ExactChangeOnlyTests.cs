using System.Collections.Generic;
using System.Runtime.InteropServices;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class ExactChangeOnlyTests
    {
        private TestBuilder _testBuilder = new TestBuilder();

        [Fact]
        public void ShouldDisplayExactChangeOnlyWhenThereArenoCoinsStocked()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            var display = sut.CheckDisplay();
            Assert.Equal("EXACT CHANGE ONLY", display);
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 0, 1)]
        public void ShouldDisplayExactChangeOnlyWhenThereIsNotEnoughChangeForAnyProduct(int quarters, int dimes, int nickels)
        {
            //Algorithm: a heuristic we will apply is to have at least 1 of every coin in the system 
            //to allow us to make change. This will cover only our current item prices.

            var sut = new VendingMachine(new StubbedDispenser());
            sut.StockCoins(new Dictionary<double, int>
            {
                {CoinWeights.Quarter, quarters},
                {CoinWeights.Dime, dimes},
                {CoinWeights.Nickel, nickels}
            });

            var display = sut.CheckDisplay();
            Assert.Equal("EXACT CHANGE ONLY", display);
        }

        [Fact]
        public void ShouldDisplayDefaultMessageWhenThereIsEnoughChangeForAnyProduct()
        {
            //Algorithm: a heuristic we will apply is to have at least 1 of every coin in the system 
            //to allow us to make change. This will cover only our current item prices.

            var sut = new VendingMachine(new StubbedDispenser());
            sut.StockCoins(new Dictionary<double, int>
            {
                {CoinWeights.Quarter, 1},
                {CoinWeights.Dime, 1},
                {CoinWeights.Nickel, 1}
            });

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Fact]
        public void ShouldDisplayExactChangeOnlyWhenButtonIsPressedAndMAchineCannotMakeChange()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            sut.StockProduct(new Dictionary<string, int> { { ProductCodes.Candy, 1 } });
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateQuarter());
            sut.InsertCoin(_testBuilder.CreateDime());
            sut.InsertCoin(_testBuilder.CreateDime());
            sut.SelectProduct(ProductCodes.Candy);

            var display = sut.CheckDisplay();
            Assert.Equal("EXACT CHANGE ONLY", display);
        }
    }
}
