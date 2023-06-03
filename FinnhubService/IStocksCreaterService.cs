using ServiceContracts.DTO;
using System;

namespace ServiceContracts
{
    public interface IStocksCreaterService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request);

        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request);
    }
}
