using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using StocksApiBasics.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace StocksApiBasics.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IFinnhubService finnhubService, IConfiguration configuration, IMemoryCache cache, ILogger<StocksController> logger)
        {
            _configuration = configuration;
            _finnhubService = finnhubService;
            _cache = cache;
            _logger = logger;
        }

        [Route("explore")]
        [Route("explore/{stockSymbol}")]
        public async Task<IActionResult> Explore(string? stockSymbol)
        {
            _logger.LogInformation("Explore of StocksController");



            // Get the list of top stocks from configuration
            var topStocksConfig = _configuration["TradingOptions:Top25PopularStocks"];
            var topStocks = topStocksConfig.Split(',').Select(s => s.Trim()).ToList();

            // Get all stocks
            //We use caching for better allocation of memory
            var cacheKey = "AllStocks";
            if (!_cache.TryGetValue(cacheKey, out List<Dictionary<string, string>> allStocks))
            {
                allStocks = await _finnhubService.GetStocks();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Adjust the expiration time as needed
                _cache.Set(cacheKey, allStocks, cacheEntryOptions);
            }

            // Filter out stocks that are not in the top stocks list
            var filteredStocks = allStocks.Where(stock => topStocks.Contains(stock["symbol"])).ToList();

            List<Stock> stocks = filteredStocks.Select(temp => new Stock()
            {
                StockSymbol = temp["displaySymbol"],
                StockName = temp["description"],
            }).ToList();
            //Here Select method is transforming each Dictionary<string, string> item in filteredStocks into a Stock object, and ToList() then collects these into a List<Stock>.


            if(!string.IsNullOrEmpty(stockSymbol))
            {
                Stock? stock = stocks.FirstOrDefault(temp => temp.StockSymbol == stockSymbol);
                Dictionary<string, object>? stockPrice = await _finnhubService.GetStockPriceQuote(stockSymbol);

                Dictionary<string, object>? companyInfo = await _finnhubService.GetCompanyProfile(stockSymbol);

                stock.CompanyLogo = companyInfo["logo"].ToString();
                stock.StockType = companyInfo["finnhubIndustry"].ToString();
                stock.StockExchange = companyInfo["exchange"].ToString();
                stock.StockPrice = Convert.ToDouble(stockPrice["c"].ToString(), CultureInfo.CurrentCulture);

                ViewBag.SingleStock = stock;
            }

            return View(stocks);
            
        }
    }
}
