using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class Assertions
    {
        public static void CoinsReturned(string scenario, int quartersReturned, int dimesReturned, int nickelsReturned,
            IList<Coin> coinReturn)
        {
            var coinsReturned = quartersReturned + dimesReturned + nickelsReturned;
            Assert.True(coinsReturned == coinReturn.Count,
                string.Format("Scenario: {0}, Expected {1} Coins, Actual {2} Coins", scenario, coinsReturned, coinReturn.Count));
            Assert.Equal(quartersReturned, coinReturn.Count(x => x.WeightInGrams == CoinWeights.Quarter));
            Assert.Equal(dimesReturned, coinReturn.Count(x => x.WeightInGrams == CoinWeights.Dime));
            Assert.Equal(nickelsReturned, coinReturn.Count(x => x.WeightInGrams == CoinWeights.Nickel));
        }
    }
}