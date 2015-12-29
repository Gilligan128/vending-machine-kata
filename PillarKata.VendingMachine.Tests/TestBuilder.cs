namespace PillarKata.VendingMachine.Tests
{
    public class TestBuilder
    {
        public VendingMachine CreateVendingMachine()
        {
            return new VendingMachine(new StubbedDispenser());
        }

        public Coin CreateQuarter()
        {
            return new Coin(CoinWeights.Quarter);
        }
    }
}