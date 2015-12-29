using System;
using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine
{
    public class CoinBag
    {
        private readonly IDictionary<double, decimal> _valueMap;
        private readonly IDictionary<double, int> _startingAmounts;

        public CoinBag(IDictionary<double, decimal> valueMap, IDictionary<double, int> startingAmounts)
        {
            _valueMap = valueMap;
            _startingAmounts = SanitizeAmounts(valueMap, startingAmounts);

        }

        public CoinBag(IDictionary<double, decimal> valueMap)
            : this(valueMap, valueMap.ToDictionary(x => x.Key, x => 0))
        {

        }

        private Dictionary<double, int> SanitizeAmounts(IDictionary<double, decimal> weightToValueMap, IDictionary<double, int> coinQuantities)
        {
            var newQuantities = coinQuantities.Where(x => weightToValueMap.ContainsKey(x.Key));
            var missingQuantities =
                weightToValueMap.Where(x => !coinQuantities.ContainsKey(x.Key))
                    .Select(x => new KeyValuePair<double, int>(x.Key, 0));
            var amounts = newQuantities.Union(missingQuantities).ToDictionary(x => x.Key, x => x.Value);
            return amounts;
        }

        public CoinBag MakeChange(decimal amount)
        {
            var amount1 = amount;
            var amountsPerCoin = new Dictionary<double, int>();
            foreach (var coin in _valueMap.OrderByDescending(x => x.Value))
            {
                var numberOfCoins = Math.Min(_startingAmounts[coin.Key], (int)(amount1 / coin.Value));
                amount1 -= numberOfCoins * coin.Value;
                amountsPerCoin.Add(coin.Key, numberOfCoins);
            }
            return new CoinBag(_valueMap, amountsPerCoin);
        }

        public bool CanMakeChange(decimal amount)
        {
            return MakeChange(amount).DollarValue == amount;
        }

        public decimal DollarValue
        {
            get { return _startingAmounts.Sum(x => x.Value * _valueMap[x.Key]); }
        }

        public bool IsEmpty
        {
            get { return DollarValue == 0; }
        }

        public CoinBag Merge(CoinBag otherBag)
        {
            if (!_startingAmounts.Keys.OrderBy(x => x).SequenceEqual(otherBag._startingAmounts.Keys.OrderBy(x => x)))
                throw new InvalidOperationException("these bags map to different coin types");

            var amounts = _startingAmounts.ToDictionary(x => x.Key, x => x.Value + otherBag._startingAmounts[x.Key]);
            return new CoinBag(_valueMap, amounts);
        }

        public IEnumerable<Coin> ToCoins()
        {
            foreach (var startingAmount in _startingAmounts)
            {
                for (int i = 0; i < startingAmount.Value; i++)
                {
                    yield return new Coin(startingAmount.Key);
                }
            }
        }
    }
}