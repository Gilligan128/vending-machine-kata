using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        private readonly IDispenseProduct _dispenser;
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

        public VendingMachine(IDispenseProduct dispenser)
        {
            _dispenser = dispenser;
        }

        public string CheckDisplay()
        {
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

        public void PressButton(string productCode)
        {
            productCode = SanitizeProductCode(productCode); //Postel's Law ;-)

            if (!_productCatalog.ContainsKey(productCode))
                return;

            var productPrice = _productCatalog[productCode];
            _currentMessage = string.Format("PRICE {0:C}", productPrice);

            var currentAmount = GetCurrentAmount();
            if (currentAmount < productPrice) return;

            _dispenser.DispenseProduct(productCode);

            _currentMessage = "THANK YOU";

            _coinsInserted.Clear();

            var currentDifference = currentAmount - productPrice;
            ReturnAppropriateChange(currentDifference);

        }

        private void ReturnAppropriateChange(decimal amountDifference)
        {
            foreach (var coin in _weightToValueMap.OrderByDescending(x => x.Value))
            {
                var numberOfCoinsToReturnForWeight = (int) (amountDifference/coin.Value);
                amountDifference -= numberOfCoinsToReturnForWeight*coin.Value;
                _coinReturn.AddRange(Enumerable.Repeat(new Coin(coin.Key), numberOfCoinsToReturnForWeight));
            }
        }

        private static string SanitizeProductCode(string productCode)
        {
            return (productCode ?? "").ToUpper();
        }

        public void StockCoins(int numberOfQuarters, int numberOfDimes, int numberOfNickels)
        {

        }
    }
}