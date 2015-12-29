﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        private readonly IDispenseProduct _dispenser;

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

        private string _currentMessage;
        private readonly Dictionary<string, int> _productStock;
        private CoinBag _coinStock;

        public VendingMachine(IDispenseProduct dispenser)
        {
            _dispenser = dispenser;
            _productStock = _productCatalog.Keys.ToDictionary(k => k, s => 0);
            _coinStock = new CoinBag(_weightToValueMap);
            _currentMessage = GetDefaultMessage();
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
            var defaultMessage = currentAmount > 0 ? string.Format("{0:C}", currentAmount) : "INSERT COINS";
            if (_coinStock.IsEmpty)
                defaultMessage = "EXACT CHANGE ONLY";
            return defaultMessage;
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

            if (_productStock[productCode] <= 0)
            {
                _currentMessage = "SOLD OUT";
                return;
            }

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
            var coinsToReturn = new CoinBag(_weightToValueMap, _coinsInserted.GroupBy(x => x.WeightInGrams, coin => 1).ToDictionary(x => x.Key, x => x.Sum()))
                .Merge(_coinStock)
                .MakeChange(amountDifference);
            
            _coinReturn.AddRange(coinsToReturn.ToCoins());
        }

        private static string SanitizeProductCode(string productCode)
        {
            return (productCode ?? "").ToUpper();
        }

        public void StockCoins(IDictionary<double, int> coinQuantities)
        {
            _coinStock = new CoinBag(_weightToValueMap, coinQuantities);
            _currentMessage = GetDefaultMessage();
        }



        public void ReturnCoins()
        {
            _coinReturn.AddRange(_coinsInserted);
        }

        public void StockProduct(IDictionary<string, int> productsStocked)
        {
            var sanitizedProductStock = productsStocked.ToDictionary(x => SanitizeProductCode(x.Key), x => x.Value);
            var validProducts = sanitizedProductStock.Where(x => _productCatalog.ContainsKey(SanitizeProductCode(x.Key)));
            foreach (var validProduct in validProducts)
            {
                _productStock[validProduct.Key] = validProduct.Value;
            }
        }
    }
}