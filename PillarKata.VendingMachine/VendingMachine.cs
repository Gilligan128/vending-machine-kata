using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        private readonly List<Coin> _coinsInserted = new List<Coin>();

        private readonly List<Coin> _coinReturn = new List<Coin>();
        private string _currentMessage = "INSERT COINS";

        private readonly List<string> _validButtonCodes = new List<string>
        {
            "COLA"
        };

        private readonly IDictionary<double, decimal> _weightToValueMap = new Dictionary<double, decimal>
        {
            {5, .05m}
            ,
            {2.27, .10m}
            ,
            {5.67, .25m}
        };

        public Display CheckDisplay()
        {
            return new Display(_currentMessage, GetCurrentAmount());
        }

        public void InsertCoin(Coin coin)
        {
            if (_weightToValueMap.ContainsKey(coin.WeightInGrams))
                _coinsInserted.Add(coin);
            else
                _coinReturn.Add(coin);
        }

        private decimal GetCurrentAmount()
        {
            return _coinsInserted.Sum(x => _weightToValueMap[x.WeightInGrams]);
        }

        public IList<Coin> GetCoinReturn()
        {
            return _coinReturn;
        }

        public void PressButton(string buttonCode)
        {
            buttonCode = (buttonCode ?? "").ToUpper(); //Postel's Law ;-)

            if (_validButtonCodes.Contains(buttonCode))
                _currentMessage = "PRICE $1.00";
        }

        public class Display
        {
            public Display(string message, decimal currentAmount)
            {
                Message = message;
                Amount = currentAmount;
            }

            public string Message { get; private set; }
            public decimal Amount { get; set; }
        }
    }
}