using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApplication.Core.Domain.IdentityEntities;
using StocksApplication.Core.DTO;
using StocksApplication.Core.ServiceContracts;

namespace StocksApplication.UI.Controllers
{

    [Route("[controller]/[action]")]
    public class BalanceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITopUpService _topUpService;

        public BalanceController(UserManager<ApplicationUser> userManager, ITopUpService topUpService)
        {
            _userManager= userManager;
            _topUpService= topUpService;
        }

        [HttpGet]
        public async Task<IActionResult> Topup()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Balance = user.Balance;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Topup(CardDTO cardDTO)
        {
            if (!ModelState.IsValid) //If there are errors while submitting card details
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);

                return View(cardDTO);
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if(cardDTO.BalanceAmount.HasValue)
            {
                bool success = await _topUpService.TopUpBalance(user.Id, cardDTO.BalanceAmount.Value); //If transaction successfull it return true

                if (success)
                {
                    ViewBag.TransactionMessage = "Successfull Transation!";
                } else
                {
                    ViewBag.TransactionMessage = "Unsuccessfull Transation!";
                }
            }

            return View();
        }
    }
}
