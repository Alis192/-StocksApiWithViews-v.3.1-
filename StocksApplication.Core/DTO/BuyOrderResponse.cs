using Entities;
using System;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) { return false; }

            if(obj.GetType() != typeof(BuyOrderResponse)) { return false; }

            BuyOrderResponse order = (BuyOrderResponse)obj;
            return this.BuyOrderID == order.BuyOrderID && this.StockSymbol == StockSymbol && this.StockName == StockName && this.DateAndTimeOfOrder == DateAndTimeOfOrder && this.Quantity == Quantity && this.Price == Price && this.TradeAmount == TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder order)
        {
            double tradeAmount = (double)Math.Round((decimal)(order.Price * order.Quantity), 2);
            return new BuyOrderResponse
            {
                BuyOrderID = order.BuyOrderID,
                StockSymbol = order.StockSymbol,
                StockName = order.StockName,
                DateAndTimeOfOrder = order.DateAndTimeOfOrder,
                Quantity = order.Quantity,
                Price = order.Price,
                TradeAmount = tradeAmount
            };
        }
    }
}
