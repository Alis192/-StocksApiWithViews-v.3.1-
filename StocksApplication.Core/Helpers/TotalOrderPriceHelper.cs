using Entities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.Helpers
{
    public class TotalOrderPriceHelper
    {
        internal static double? CalculateOrderPrice(BuyOrder _buyOrder)
        {
            double? total = _buyOrder.Price * _buyOrder.Quantity;
            return total;
        }
    }
}
