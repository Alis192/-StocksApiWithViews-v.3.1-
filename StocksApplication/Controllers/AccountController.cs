using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApiBasics.Controllers;
using StocksApplication.Core.Domain.IdentityEntities;
using StocksApplication.Core.DTO;

namespace StocksApplication.UI.Controllers
{

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; //Will manipulate user accounts such as Create, Update, Delete and Searching
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            //Check for Validation errors
            if (ModelState.IsValid == false)
            {
                //From the model State we have to get all the values of all the properties one by one and each property may have one or more error messages, so we are selecting all those error messages here and from those error objects we have to get only error message property, so all the error messages means the string values will be added into the ViewBag.Errors
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage); //We 
                return View(registerDTO);
            }

            //In case no errros
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            //We have to supply password while calling CreateAsync() method

            //This repository layer which eventually performs the relevant operation in the DB context for example internally it adds the model object to the DB set and calls the save changes method
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //Sign in
                await _signInManager.SignInAsync(user, isPersistent: false); //Will create a cookie and send to browser showing that the current user has signed in

                return RedirectToAction(nameof(TradeController.Index), "Trade");
            } 
            else //If result doesn't succeed, we display errors
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description); //Adding errros to Model object, which later be sent to the View
                }

                return View(registerDTO);
            }
        } 


        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (ModelState.IsValid == false) //When we have errors while logging in
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage); //We 
                return View(loginDTO);
            }

            //When no errors are present
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded) //If log in is successfull
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(TradeController.Index), "Trade");
            }

            ModelState.AddModelError("Login", "Invalid email or password"); //In case of wrong login details

            return View(loginDTO);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }
    }
}
