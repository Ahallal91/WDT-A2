using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using A2.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using A2.Data;
using SimpleHashing;

namespace A2.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<A2User> _userManager;
        private readonly SignInManager<A2User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IdentityA2Context _context;

        public LoginModel(SignInManager<A2User> signInManager,
            ILogger<LoginModel> logger,
            UserManager<A2User> userManager,
            IdentityA2Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(8)]
            [RegularExpression(@"^\d{8}?|(admin)$", ErrorMessage = "Login ID must be 8 digits.")]
            [Display(Name = "Login ID")]
            public string UserID { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        /// <summary>
        /// Logs a user in via usermanager, if users are a part of the old system it uses SimpleHashing to verify and login the user
        /// rather than IdentityAPI hashing algorithm. Admin accounts are redirected to the admin project
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var user = await _userManager.FindByIdAsync(Input.UserID);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt1.");
            }
            if (user.Status == ActiveType.Locked)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked, please contact an Administrator.");
            }
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var result = await _signInManager.PasswordSignInAsync(Input.UserID, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    // handle legacy hashed password from A1.
                    if (PBKDF2.Verify(user.PasswordHash, Input.Password))
                    {
                        await _signInManager.SignInAsync(user, false, default);
                        _logger.LogInformation("User logged in.");
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return Page();
                    }
                }
                if (result.Succeeded)
                {
                    var getRoles = _userManager.GetRolesAsync(user).Result;
                    foreach (var role in getRoles)
                    {
                        if (role == "Admin")
                        {
                            return Redirect("https://localhost:44350/");
                        }
                        else if (role == "Customer")
                        {
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                        }
                    }
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt2");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
