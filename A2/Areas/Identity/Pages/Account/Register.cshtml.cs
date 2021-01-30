using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using A2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using A2.Data;
using A2.Models;
using A2.Controllers.BusinessObject;

namespace A2.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<A2User> _signInManager;
        private readonly UserManager<A2User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IdentityA2Context _context;

        public RegisterModel(
            UserManager<A2User> userManager,
            SignInManager<A2User> signInManager,
            ILogger<RegisterModel> logger,
            IdentityA2Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(8)]
            [RegularExpression(@"^\d{8}?$", ErrorMessage = "Login ID must be 8 digits.")]
            [Display(Name = "Login ID")]
            public string LoginID { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Ente a valid name")]
            [StringLength(50)]
            public string CustomerName { get; set; }
            [Required]
            [RegularExpression(@"^[+]?(61)\s\d{4}\s\d{4}$", ErrorMessage = "Enter a valid phone number.")]
            [StringLength(15)]
            public string Phone { get; set; }
            [Required]
            [StringLength(1)]
            [Display(Name = "Type")]
            [RegularExpression(@"^[SC]$", ErrorMessage = "Enter a valid account type")]
            public string AccountType { get; set; }
            [Required]
            [Column(TypeName = "money")]
            [DataType(DataType.Currency)]
            [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
            public decimal Balance { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var accountConstants = new AccountConstraints(Input.AccountType);
            if (Input.Balance < accountConstants.OpeningBalance)
            {
                ModelState.AddModelError("AccountError", $"You must have a minimum deposit of ${accountConstants.OpeningBalance} for account type {Input.AccountType}");
            }
            if (ModelState.IsValid)
            {
                var user = new A2User { UserName = Input.LoginID, Id = Input.LoginID, Status = ActiveType.Unlocked, ModifyDate = DateTime.UtcNow };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var customerID = GenerateRandomCustID().Result;
                    _context.Customer.Add(new Customer()
                    {
                        CustomerID = customerID,
                        CustomerName = Input.CustomerName,
                        Phone = Input.Phone
                    });
                    var accountID = GenerateRandomAccID().Result;
                    _context.Account.Add(new Models.Account()
                    {
                        AccountNumber = accountID,
                        AccountType = Input.AccountType,
                        CustomerID = customerID,
                        Balance = Input.Balance,
                        ModifyDate = DateTime.UtcNow
                    });
                    _context.SaveChanges();
                    user.CustomerID = customerID;
                    await _userManager.UpdateAsync(user);
                    await _userManager.AddToRoleAsync(user, "Customer");
                    _logger.LogInformation("User created a new account with password.");

                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Home", "Customer");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<int> GenerateRandomCustID()
        {
            var randomNumberGenerator = new Random();
            int num = randomNumberGenerator.Next(1000, 9999);
            bool ValidNumber = true;
            while (ValidNumber)
            {
                var getCustomerID = await _context.Customer.FindAsync(num);
                if (getCustomerID == null)
                {
                    ValidNumber = false;
                }
            }
            return num;
        }
        public async Task<int> GenerateRandomAccID()
        {
            var randomNumberGenerator = new Random();
            int num = randomNumberGenerator.Next(1000, 9999);
            bool ValidNumber = true;
            while (ValidNumber)
            {
                var getAccountID = await _context.Account.FindAsync(num);
                if (getAccountID == null)
                {
                    ValidNumber = false;
                }
            }
            return num;
        }
    }
}
