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

namespace Services
{
    public class StocksGetterService : IStocksGetterService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksCreaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext; //to read data in the log
        public StocksGetterService(IStocksRepository stocksRepository, ILogger<StocksCreaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            _logger.LogInformation("GetBuyOrders of StocksService");


            List<BuyOrder> orders_from_db =  await _stocksRepository.GetBuyOrders();
            return orders_from_db.Select(buyOrders => buyOrders.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {


            _logger.LogInformation("GetSellOrders of StocksService");

            List<SellOrder> orders_from_db;
            using (Operation.Time("Time for Retrieving GetSellOrders from database"))
            {
                orders_from_db = await _stocksRepository.GetSellOrders();
            }

            return orders_from_db.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
        }
    }
}
