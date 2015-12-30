using System;
using System.Collections.Generic;
using System.Linq;

namespace PillarKata.VendingMachine
{
    public class CoinBag
    {
        private readonly IDictionary<double, decimal> _valueMap;
        private readonly IDictionary<double, int> _coinQuantities;

        public CoinBag(IDictionary<double, decimal> valueMap, IDictionary<double, int> startingAmounts)
        {
            _valueMap = valueMap;
            _coinQuantities = SanitizeAmounts(valueMap, startingAmounts);

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
                var numberOfCoins = Math.Min(_coinQuantities[coin.Key], (int)(amount1 / coin.Value));
                amount1 -= numberOfCoins * coin.Value;
                amountsPerCoin.Add(coin.Key, numberOfCoins);
            }
            return new CoinBag(_valueMap, amountsPerCoin);
        }

        public CoinBag WithCoins(IEnumerable<Coin> coins)
        {
            var coinGroups = coins.GroupBy(x => x.WeightInGrams, coin => 1);
            var newQuantities = new Dictionary<double, int>(_coinQuantities);
            foreach (var coinGroup in coinGroups)
            {
                newQuantities[coinGroup.Key] += coinGroup.Sum();
            }

            return new CoinBag(_valueMap, newQuantities);
        }

        public bool CanMakeChange(decimal amount)
        {
            return MakeChange(amount).DollarValue == amount;
        }

        public decimal DollarValue
        {
            get { return _coinQuantities.Sum(x => x.Value * _valueMap[x.Key]); }
        }

        public bool IsEmpty
        {
            get { return DollarValue == 0; }
        }

        public bool AnySlotEmpty
        {
            get { return _coinQuantities.Any(x => x.Value <= 0); }
        }

        public CoinBag Merge(CoinBag otherBag)
        {
            if (!_coinQuantities.Keys.OrderBy(x => x).SequenceEqual(otherBag._coinQuantities.Keys.OrderBy(x => x)))
                throw new InvalidOperationException("these bags map to different coin types");

            var amounts = _coinQuantities.ToDictionary(x => x.Key, x => x.Value + otherBag._coinQuantities[x.Key]);
            return new CoinBag(_valueMap, amounts);
        }

        public IEnumerable<Coin> ToCoins()
        {
            foreach (var startingAmount in _coinQuantities)
            {
                for (int i = 0; i < startingAmount.Value; i++)
                {
                    yield return new Coin(startingAmount.Key);
                }
            }
        }
    }
}