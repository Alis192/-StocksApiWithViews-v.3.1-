using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using RepositoryContracts;

namespace Services
{
    public class StocksService : IStocksService
    {
        //private readonly List<BuyOrder> _orders;
        //private readonly List<SellOrder> _sellOrders;
        private readonly IStocksRepository _stocksRepository;

        public StocksService(IStocksRepository stocksRepository)
        {
            //_orders = new List<BuyOrder>();
            //_sellOrders = new List<SellOrder>();
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null) throw new ArgumentNullException();

            ValidationHelpers.ModelValidation(request);


            BuyOrder buy_order = request.ToBuyOrder();

            buy_order.BuyOrderID = Guid.NewGuid();

            buy_order.DateAndTimeOfOrder= DateTime.Now;

            await _stocksRepository.CreateBuyOrder(buy_order); //calling repository method

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

            try
            {
                await _stocksRepository.CreateSellOrder(order_from_request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding SellOrder: {ex.Message}");
                throw;
            }

            //_db.SellOrders.Add(order_from_request);
            //await _db.SaveChangesAsync();

            //SellOrderResponse sell_order_response = order_from_request.ToSellOrderResponse();

            return order_from_request.ToSellOrderResponse();

        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> orders_from_db =  await _stocksRepository.GetBuyOrders();
            return orders_from_db.Select(buyOrders => buyOrders.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> orders_from_db = await _stocksRepository.GetSellOrders();
            return orders_from_db.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
        }
    }
}
