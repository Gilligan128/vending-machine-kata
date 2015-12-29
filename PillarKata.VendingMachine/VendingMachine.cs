using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        private readonly List<Coin> _coinsInserted = new List<Coin>();

        private readonly List<Coin> _coinReturn = new List<Coin>();
        private string _currentMessage = "INSERT COINS";

        private readonly  IDictionary<string, decimal> _productCatalog = new Dictionary<string, decimal>
        {
            {"COLA", 1m},
            {"CHIPS", .5m},
            {"CANDY", .65m}
        }; //TODO: Refactor this into an injected map.

        private readonly IDictionary<double, decimal> _weightToValueMap = new Dictionary<double, decimal>
        {
            {5, .05m},
            {2.27, .10m},
            {5.67, .25m}
        }; 

        public Display CheckDisplay()
        {
            var display = new Display(_currentMessage, GetCurrentAmount());
            _currentMessage = "INSERT COINS";
            return display;
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

            if (_productCatalog.ContainsKey(buttonCode))
                _currentMessage = string.Format("PRICE {0:C}", _productCatalog[buttonCode]);
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