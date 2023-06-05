using Entities;
using System;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmount { get; set; }

        public override bool Equals(object? obj) //we override Equals() method, because by default it compares instance of 2 objects, not their values
        {
            if (obj == null) { return false; }

            if (obj.GetType() != typeof(SellOrderResponse)) { return false; }

            SellOrderResponse order = (SellOrderResponse)obj;
            return this.SellOrderID == order.SellOrderID && this.StockSymbol == StockSymbol && this.StockName == StockName && this.DateAndTimeOfOrder == DateAndTimeOfOrder && this.Quantity == Quantity && this.Price == Price && this.TradeAmount == TradeAmount;
        }

        public override int GetHashCode() //we also override GetHashCode() to get rid of warning 
        {
            return base.GetHashCode();
        }

    }

    /// <summary>
    /// This is an external method of SellOrder. Since it is not good idea to write codes in the SellOrder.cs base class. So we create a method of it outside its domain
    /// </summary>
    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder order)
        {
            double tradeAmount = (double)Math.Round((decimal)(order.Price * order.Quantity), 2);
            return new SellOrderResponse
            {
                SellOrderID = order.SellOrderID,
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
     