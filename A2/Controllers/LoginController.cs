using A2.Areas.Identity.Data;
using A2.Areas.Identity.Pages.Account;
using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers
{
    /*
    * Reference McbaExampleWithLogin Login.cs week 6
    */
    public class LoginController : Controller
    {
        private readonly IdentityA2Context _context;
        private readonly UserManager<A2User> _userManager;
        private readonly SignInManager<A2User> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IdentityA2Context context,
            UserManager<A2User> userManager,
            SignInManager<A2User> signInManager,
            ILogger<LoginController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

        }
        public string ReturnURL { get; set; }

        public IActionResult Login() => View();

        public async Task<IActionResult> OnPostAsync(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer { CustomerID = 1234, CustomerName = model.Input.CustomerName, Phone = "+61 9999 9999" };
                var account = new Models.Account { AccountNumber = 1234, AccountType = "S", Balance = 10, ModifyDate = DateTime.UtcNow };
                var user = new A2User { Id = model.Input.LoginID, UserName = model.Input.LoginID };

                _context.Customer.Add(customer);

                _context.Account.Add(account);
                _context.SaveChanges();
                var result = await _userManager.CreateAsync(user, model.Input.Password);

                Console.WriteLine(result);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("", "Customer");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return model.Page();
        }
        /*[HttpPost]
        public async Task<IActionResult> Login(string loginID, string password)
        {
            var login = await _context..FindAsync(loginID);
            if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Invalid LoginID or password, please try again.");
                return View(new Login { LoginID = loginID });
            }
            if (login.Status == ActiveType.Locked)
            {
                ModelState.AddModelError("LoginFailed", "Your account has been locked, please contact the Administrator.");
                return View(new Login { LoginID = loginID });
            }

            // Set customerID and customerName in the session after sucessful login.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);

            return RedirectToAction("Home", "Customer");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }*/
    }
}
