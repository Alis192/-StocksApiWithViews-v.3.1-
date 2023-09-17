using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using System.Runtime.InteropServices;
using StocksApplication.Core.Domain.RepositoryContracts;


namespace StocksApplication.Infrastructure.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly OrdersDbContext _db;

        public StocksRepository(OrdersDbContext db)
        {
            _db = db;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task DeleteAnOrder(Guid orderId)
        {
            BuyOrder? order = await _db.BuyOrders.SingleOrDefaultAsync(o => o.BuyOrderID == orderId);
            if (order != null)
            {
                _db.BuyOrders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<BuyOrder>> GetBuyOrders(Guid userId)
        {
            List<BuyOrder> all_buy_orders = await _db.BuyOrders.Where(orders => orders.UserId == userId).ToListAsync();
            return all_buy_orders;
        }

        public async Task<List<SellOrder>> GetSellOrders(Guid userId)
        {
            List<SellOrder> all_sell_orders = await _db.SellOrders.Where(orders => orders.UserId == userId).ToListAsync();
            return all_sell_orders;
        }

        public async Task<BuyOrder?> GetSingleBuyOrder(Guid orderId)
        {
            BuyOrder? buyOrder =  await _db.BuyOrders.Where(order => order.BuyOrderID.Equals(orderId)).SingleOrDefaultAsync();
            return buyOrder;
        }
    }
}
