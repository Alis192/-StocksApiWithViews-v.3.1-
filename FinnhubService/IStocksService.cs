using ServiceContracts.DTO;
using System;

namespace ServiceContracts
{
    public interface IStocksService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request);

        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request);

        Task<List<BuyOrderResponse>> GetBuyOrders();

        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
