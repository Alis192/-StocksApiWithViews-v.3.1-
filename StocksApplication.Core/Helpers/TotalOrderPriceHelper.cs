using Entities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.Helpers
{
    public class TotalOrderPriceHelper
    {
        internal static double? CalculateOrderPriceBuyOrder(BuyOrder _buyOrder)
        {
            double? total = _buyOrder.Price * _buyOrder.Quantity;
            return total;
        }

        //internal static double? CalculateOrderPriceSellOrder(SellOrder _sellOrder)
        //{
        //    double? total = _sellOrder.Price * _sellOrder.Quantity;
        //    return total;
        //}
    }
}
