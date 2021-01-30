using A2.Areas.Identity.Data;
using A2.Areas.Identity.Pages.Account;
using A2.Data;
using A2.Models;
using A2.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IdentityA2Context _context;
        private readonly UserManager<A2User> _userManager;
        private readonly SignInManager<A2User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IdentityA2Context context,
            UserManager<A2User> userManager,
            SignInManager<A2User> signInManager,
            ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

        }
        [Route("Account")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer { CustomerID = 1234, CustomerName = model.CustomerName, Phone = "+61 9999 9999" };
                var account = new Account { AccountNumber = 1234, AccountType = "S", Balance = 10, ModifyDate = DateTime.UtcNow };
                var user = new A2User { Id = model.LoginID, UserName = model.LoginID };

                _context.Customer.Add(customer);
                _context.Account.Add(account);
                _context.SaveChanges();

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Home", "Customer");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

    }
}
