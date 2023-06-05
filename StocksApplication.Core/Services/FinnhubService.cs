using ServiceContracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net;

using StocksApplication.Core.Domain.RepositoryContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IConfiguration configuration, IFinnhubRepository finnhubRepository)
        {
            _configuration = configuration;
            _finnhubRepository= finnhubRepository;
        }


        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {

            Dictionary<string, object> response = await _finnhubRepository.GetCompanyProfile(stockSymbol); //repository call

            return response;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            Dictionary<string, object> response = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            return response;
        }

        public async Task<List<Dictionary<string, string>>> GetStocks()
        {
            List<Dictionary<string, string>> allStocks = await _finnhubRepository.GetStocks();
            return allStocks;
        }
    }
}