namespace PillarKata.VendingMachine
{
    public class Coin
    {
        public int WeightInGrams { get; private set; }

        public Coin(int weightInGrams)
        {
            WeightInGrams = weightInGrams;
        }
    }
}