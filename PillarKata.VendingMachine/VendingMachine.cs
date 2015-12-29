using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        private const string DefaultMessageWithoutCoins = "INSERT COINS";

        private readonly List<Coin> _coinReturn = new List<Coin>();
        private readonly List<Coin> _coinsInserted = new List<Coin>();

        private readonly IDictionary<string, decimal> _productCatalog = new Dictionary<string, decimal>
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

        private string _currentMessage = DefaultMessageWithoutCoins;

        public string CheckDisplay()
        {
            ;
            var display = _currentMessage;
            _currentMessage = GetDefaultMessage();
            return display;
        }

        public void InsertCoin(Coin coin)
        {
            if (_weightToValueMap.ContainsKey(coin.WeightInGrams))
            {
                _coinsInserted.Add(coin);
                _currentMessage = GetDefaultMessage();
            }
            else
                _coinReturn.Add(coin);
        }

        private string GetDefaultMessage()
        {
            var currentAmount = GetCurrentAmount();
            return currentAmount > 0 ? string.Format("{0:C}", currentAmount) : DefaultMessageWithoutCoins;
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
    }
}