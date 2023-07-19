using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApplication.Core.Domain.IdentityEntities;

namespace StocksApplication.UI.ViewComponents
{
    public class BalanceViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BalanceViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            double? balance = Math.Round(user.Balance, 2);

            return View(balance);
        }
    }
}
