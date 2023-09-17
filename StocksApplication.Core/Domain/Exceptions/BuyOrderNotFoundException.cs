using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.Domain.Exceptions
{
    public class BuyOrderNotFoundException : Exception
    {
        public BuyOrderNotFoundException(string message) :  base(message) { }

    }
}
