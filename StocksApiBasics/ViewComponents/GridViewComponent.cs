using Microsoft.AspNetCore.Mvc;
using StocksApiBasics.Models;

namespace StocksApiBasics.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Stock stock)
        {
            return View(stock);
        }
    }
}
