using System.Collections.Generic;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class ExactChangeOnlyTests
    {
        [Fact]
        public void ShouldDisplayExactChangeOnlyWhenThereArenoCoinsStocked()
        {
            var sut = new VendingMachine(new StubbedDispenser());

            var display = sut.CheckDisplay();
            Assert.Equal("EXACT CHANGE ONLY", display);
        }

        [Theory]
        [InlineData(1,1,0)]
        [InlineData(1,0,1)]
        [InlineData(0,0,1)]
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
    }
}
