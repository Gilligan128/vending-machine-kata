namespace PillarKata.VendingMachine
{
    public class Coin
    {
        public double WeightInGrams { get; private set; }

        public Coin(double weightInGrams)
        {
            WeightInGrams = weightInGrams;
        }
    }
}