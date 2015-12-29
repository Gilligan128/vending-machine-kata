using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine.Tests
{
    public class TestBuilder
    {
        public VendingMachine CreateVendingMachine()
        {
            var vendingMachine = new VendingMachine(new StubbedDispenser());
            vendingMachine.StockProduct(new Dictionary<string, int>
            {
                {ProductCodes.Cola, 1},
                {ProductCodes.Candy, 1},
                {ProductCodes.Chips,1}
            });
            vendingMachine.StockCoins(new Dictionary<double, int>
            {
                {CoinWeights.Quarter, 3},
                {CoinWeights.Nickel, 2},
                {CoinWeights.Dime, 2},

            } );
            return vendingMachine;
        }

        public Coin CreateQuarter()
        {
            return Quarter();
        }

        private static Coin Quarter()
        {
            return new Coin(CoinWeights.Quarter);
        }

        public Coin CreateDime()
        {
            return new Coin(CoinWeights.Dime);
        }

        public Coin CreateNickel()
        {
            return new Coin(CoinWeights.Nickel);
        }
    }
}