using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using StocksApplication.Core.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Services
{
    public class StocksGetterService : IStocksGetterService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksCreaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext; //to read data in the log
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StocksGetterService(IStocksRepository stocksRepository, ILogger<StocksCreaterService> logger, IDiagnosticContext diagnosticContext, IHttpContextAccessor httpContextAccessor)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            _logger.LogInformation("GetBuyOrders of StocksService");

            Guid userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<BuyOrder> orders_from_db =  await _stocksRepository.GetBuyOrders(userId);

            return orders_from_db.Select(buyOrders => buyOrders.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            _logger.LogInformation("GetSellOrders of StocksService");

            Guid userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<SellOrder> orders_from_db;
            using (Operation.Time("Time for Retrieving GetSellOrders from database"))
            {
                orders_from_db = await _stocksRepository.GetSellOrders(userId);
            }

            return orders_from_db.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
        }

        //public async Task<BuyOrderResponse> GetSingleBuyOrder(Guid id)
        //{
        //    BuyOrder? buyOrder = await _stocksRepository.GetSingleBuyOrder(id);

        //    return buyOrder == null ? throw new ArgumentNullException(nameof(buyOrder)) : buyOrder.ToBuyOrderResponse();



        //}
    }
}
