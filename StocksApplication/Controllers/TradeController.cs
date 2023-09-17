using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using Microsoft.Extensions.Options;
using StocksApiBasics.Models;
using System.Text.Json;
using System.Globalization;
using ServiceContracts.DTO;
using Rotativa.AspNetCore;
using StocksApiBasics.Filters.ActionFilters;
using Microsoft.AspNetCore.Identity;
using StocksApplication.Core.Domain.IdentityEntities;
using StocksApplication.Core.Domain.Exceptions;

namespace StocksApiBasics.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly StocksApiOptions _options; //private field to create options method for StocksApiOptions.cs
        private readonly IConfiguration _config;
        private readonly IStocksGetterService _stocksGetterService;
        private readonly IStocksCreaterService _stocksCreaterService;
        
        public TradeController(IFinnhubService finnhubService, IOptions<StocksApiOptions> stocksApiOptions, IConfiguration configuration, IStocksGetterService stocksGetterService, IStocksCreaterService stocksCreaterService) //so 'stocksApiOptions' parameter get an object if IOptions
        {
            _finnhubService = finnhubService;

            _options = stocksApiOptions.Value; //and it has a predefined property called value which contains an object of stocksApiOptions class                                                    
            //stocksApiOptions is IOptions<> type, Value is 'StocksApiOptions' type
            _config= configuration; // we obtain user token 
            _stocksGetterService= stocksGetterService; //creating service object
            _stocksCreaterService= stocksCreaterService;
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
            ViewBag.BalanceError = TempData["BalanceError"];
            ViewBag.EmptyBuyOrder = TempData["EmptyBuyOrder"];
            

            ViewBag.FinnhubToken = _config["userToken"]; //sending userToken to the view because we use it in JS file to update prices 

            //Retrieving user balance from database and display it in the View          
            
            return View(stockTrade);
        }

        [HttpPost]
        [Route("Buyorder")]
        [TypeFilter(typeof(BuyAndSellOrderErrorValidating))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest order)
        {
            //if(!ModelState.IsValid)
            //{
            //    TempData["errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return RedirectToAction("Index", "Home", ViewBag.Errors);
            //}

            try
            {
                await _stocksCreaterService.CreateBuyOrder(order);
            }
            catch (InsufficientBalanceException ex)
            {
                TempData["BalanceError"] = ex.Message;
                //ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Index));
            }



            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [Route("sellorder/{buyOrderId}")]
        //[TypeFilter(typeof(BuyAndSellOrderErrorValidating))]
        public async Task<IActionResult> SellOrder(Guid buyOrderId)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return RedirectToAction("Index", "Home", ViewBag.Errors);
            //}

            

            try
            {
                await _stocksCreaterService.CreateSellOrder(buyOrderId);
            } 
            catch(BuyOrderNotFoundException ex) 
            {
                TempData["EmptyBuyOrder"] = ex.Message;
                //ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Index));
            }


            

            //await _stocksCreaterService.CreateSellOrder(orderId);

            return RedirectToAction(nameof(Orders));
        }




        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> orders_from_list_buy = await _stocksGetterService.GetBuyOrders();

            List<SellOrderResponse> order_from_list_sell = await _stocksGetterService.GetSellOrders();
            
            Orders orders = new Orders();
            
            orders.BuyOrders = orders_from_list_buy;
            orders.SellOrders = order_from_list_sell;
            
            return View(orders);
        }


        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<BuyOrderResponse> buyOrders = await _stocksGetterService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksGetterService.GetSellOrders();

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