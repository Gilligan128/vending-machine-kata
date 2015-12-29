using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillarKata.VendingMachine
{
    public class VendingMachine
    {
        public Display CheckDisplay()
        {
            return new Display("INSERT COINS");
        }

        public class Display
        {
            public string Message { get; private set; }
            public decimal Amount { get; set; }

            public Display(string message)
            {
                Message = message;
            }
        }
    }

    
}
