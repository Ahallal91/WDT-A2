using A2.Data;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers
{
    [Route("/MCBA/CustomerLogin)")]
    public class LoginController : Controller
    {
        private readonly A2Context _context;
        public LoginController(A2Context context) => _context = context;
        public IActionResult Login() => View();
        [HttpPost]
        public async IActionResult Login(string loginID, string password)
        {
            var login = await _context.Login.FindAsync(loginID);
            if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Your details are incorrect, please try again.");
                return;
            }

            // Customers details accepted, create session.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);

            return RedirectToAction("Index", "Customer");
        }
    }
}
