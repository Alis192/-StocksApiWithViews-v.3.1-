using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using Microsoft.Extensions.Options;
using StocksApiBasics.Models;
using System.Text.Json;
using System.Globalization;
using ServiceContracts.DTO;
using Rotativa.AspNetCore;

namespace StocksApiBasics.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly StocksApiOptions _options; //private field to create options method for StocksApiOptions.cs
        private readonly IConfiguration _config;
        private readonly IStocksService _stocksService;

        
        public TradeController(IFinnhubService finnhubService, IOptions<StocksApiOptions> stocksApiOptions, IConfiguration configuration, IStocksService stocksService) //so 'stocksApiOptions' parameter get an object if IOptions
        {
            _finnhubService = finnhubService;

            _options = stocksApiOptions.Value; //and it has a predefined property called value which contains an object of stocksApiOptions class                                                    
            //stocksApiOptions is IOptions<> type, Value is 'StocksApiOptions' type
            _config= configuration; // we obtain user token 
            _stocksService= stocksService; //creating service object
        }



        [Route("/")]
        [Route("/Trade")]
        [Route("/Trade/Index/{stockSymbol}")]
        [HttpGet]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol ))
            {
                stockSymbol = _options.DefaultStockSymbol;
            }
            uint? defaultOrderQuantity = _options.DefaultOrderQuantity;


            //if (string.IsNullOrEmpty(stockSymbol))
            //{
            //    stockSymbol = "MSFT";
            //}
            Dictionary<string, object>? profile = await _finnhubService.GetCompanyProfile(stockSymbol); //returns json data about company
            Dictionary<string, object>? price = await _finnhubService.GetStockPriceQuote(stockSymbol); //returns json data about stocks

            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = stockSymbol,
                StockName = profile.ContainsKey("name") ? profile["name"].ToString() : null,
                Price = Convert.ToDouble(price?["c"].ToString(), CultureInfo.CurrentCulture), //culture info is used to read decimal dot in the data
                Quantity = defaultOrderQuantity
            }; 

            ViewBag.Errors = TempData["errors"]; //tempdata is used to pass errors from one controller method to another one

            ViewBag.FinnhubToken = _config["userToken"]; //sending userToken to the view because we use it in JS file to update prices 
            return View(stockTrade);
        }

        [HttpPost]
        [Route("Buyorder")]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest order)
        {
            if(!ModelState.IsValid)
            {
                TempData["errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("Index", "Home", ViewBag.Errors);
            }

            await _stocksService.CreateBuyOrder(order);

            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [Route("Sellorder")]
        public async Task<IActionResult> SellOrder(SellOrderRequest order)
        {
            if (!ModelState.IsValid)
            {
                TempData["errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("Index", "Home", ViewBag.Errors);
            }

            await _stocksService.CreateSellOrder(order);

            return RedirectToAction(nameof(Orders));
        }




        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> orders_from_list_buy = await _stocksService.GetBuyOrders();

            List<SellOrderResponse> order_from_list_sell = await _stocksService.GetSellOrders();
            
            Orders orders = new Orders();
            
            orders.BuyOrders = orders_from_list_buy;
            orders.SellOrders = order_from_list_sell;
            
            return View(orders);
        }


        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            Orders order = new Orders();
            order.BuyOrders = buyOrders;
            order.SellOrders = sellOrders;

            return new ViewAsPdf("OrdersPDF", order, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20,
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

    }
}