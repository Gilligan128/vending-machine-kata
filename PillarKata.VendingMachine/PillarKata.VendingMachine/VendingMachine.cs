using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        private readonly List<Coin> _coinsInserted = new List<Coin>();

        public Display CheckDisplay()
        {
            return new Display("INSERT COINS", GetCurrentAmount());
        }

        public class Display
        {
            public string Message { get; private set; }
            public decimal Amount { get; set; }

            public Display(string message, decimal currentAmount)
            {
                Message = message;
                Amount = currentAmount;
            }
        }

        public void InsertCoin(Coin coin)
        {
            _coinsInserted.Add(coin);
        }

        private decimal GetCurrentAmount()
        {
           return _coinsInserted.Sum(x => .05m);
        }
    }

    
}
