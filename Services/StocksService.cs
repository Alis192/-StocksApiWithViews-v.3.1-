using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using Microsoft.EntityFrameworkCore;

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

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null) throw new ArgumentNullException();

            ValidationHelpers.ModelValidation(request);


            BuyOrder buy_order = request.ToBuyOrder();

            buy_order.BuyOrderID = Guid.NewGuid();

            buy_order.DateAndTimeOfOrder= DateTime.Now;

            _db.BuyOrders.Add(buy_order);
            await _db.SaveChangesAsync();

            //BuyOrderResponse order_response = buy_order.ToBuyOrderResponse();
            
            return buy_order.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request)  
        {
            if (request == null) { throw new ArgumentNullException(); }

            ValidationHelpers.ModelValidation(request);

            SellOrder order_from_request = request.ToSellOrder();

            order_from_request.SellOrderID = Guid.NewGuid();

            order_from_request.DateAndTimeOfOrder = DateTime.Now;

            _db.SellOrders.Add(order_from_request);
            await _db.SaveChangesAsync();

            //SellOrderResponse sell_order_response = order_from_request.ToSellOrderResponse();

            return order_from_request.ToSellOrderResponse();

        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrders = await _db.BuyOrders.ToListAsync();
            return buyOrders.Select(buyorder => buyorder.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrders = await _db.SellOrders.ToListAsync();
            return sellOrders.Select(sellorder => sellorder.ToSellOrderResponse()).ToList();

        }
    }
}
