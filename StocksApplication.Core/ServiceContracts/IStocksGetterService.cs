using ServiceContracts.DTO;
using System;

namespace ServiceContracts
{
    public interface IStocksGetterService
    {
        Task<List<BuyOrderResponse>> GetBuyOrders();

        Task<List<SellOrderResponse>> GetSellOrders();

    }
}
