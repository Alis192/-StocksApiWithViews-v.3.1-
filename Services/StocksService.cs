using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        //private readonly List<BuyOrder> _orders;
        //private readonly List<SellOrder> _sellOrders;
        private readonly OrdersDbContext _db; 

        public StocksService(OrdersDbContext ordersDbContext)
        {
            //_orders = new List<BuyOrder>();
            //_sellOrders = new List<SellOrder>();
            _db = ordersDbContext;
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null) throw new ArgumentNullException();

            ValidationHelpers.ModelValidation(request);


            BuyOrder buy_order = request.ToBuyOrder();

            buy_order.BuyOrderID = Guid.NewGuid();

            buy_order.DateAndTimeOfOrder= DateTime.Now;

            _db.BuyOrders.Add(buy_order);
            _db.SaveChanges();

            //BuyOrderResponse order_response = buy_order.ToBuyOrderResponse();
            
            return buy_order.ToBuyOrderResponse();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? request)  
        {
            if (request == null) { throw new ArgumentNullException(); }

            ValidationHelpers.ModelValidation(request);

            SellOrder order_from_request = request.ToSellOrder();

            order_from_request.SellOrderID = Guid.NewGuid();

            order_from_request.DateAndTimeOfOrder = DateTime.Now;

            _db.SellOrders.Add(order_from_request);
            _db.SaveChanges();

            //SellOrderResponse sell_order_response = order_from_request.ToSellOrderResponse();

            return order_from_request.ToSellOrderResponse();

        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            //List<BuyOrderResponse> buy_order_list = new List<BuyOrderResponse>();
            //foreach (BuyOrder order in _orders)
            //{
            //    buy_order_list.Add(order.ToBuyOrderResponse());
            //}
            //return buy_order_list;

            return _db.BuyOrders.ToList().Select(buyorder => buyorder.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            //List<SellOrderResponse> sell_order_list = new List<SellOrderResponse>();
            //foreach (SellOrder order in _sellOrders)
            //{
            //    sell_order_list.Add(order.ToSellOrderResponse());
            //}
            //return sell_order_list;
            return _db.SellOrders.ToList().Select(sellorder => sellorder.ToSellOrderResponse()).ToList();

        }
    }
}
