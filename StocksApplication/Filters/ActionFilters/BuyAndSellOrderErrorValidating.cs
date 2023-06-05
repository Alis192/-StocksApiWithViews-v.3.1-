using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StocksApiBasics.Controllers;
using System;

namespace StocksApiBasics.Filters.ActionFilters
{

    public class BuyAndSellOrderErrorValidating : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is TradeController tradeController)
            {
                if (!tradeController.ModelState.IsValid)
                {
                    tradeController.TempData["errors"] = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    context.Result = new RedirectToActionResult("Index", "Trade", null); //returning View() of filter 
                } 
                else
                {
                    await next(); //calls the subsequent filter or action method
                }
            }
            else //when controller type is not Person, we again call next() delegate
            {
                await next(); //calls the subsequent filter or action method
            }

        }
    }
}
