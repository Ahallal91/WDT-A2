using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Admin.Util;
using SimpleHashing;
using Microsoft.AspNetCore.Http;
using Admin.Filters;

namespace Admin.Controllers
{
    /*
    * Reference McbaExampleWithLogin Login.cs week 6
    */
    [Route("")]
    public class LoginController : Controller
    {
        private const string adminID = "admin";
        private const string adminPW = "admin";
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult GetLogin(Administrator admin)
        {
            if (admin.AdminID != adminID || admin.AdminPW != adminPW)
            {
                ModelState.AddModelError("LoginFailed", "Invalid LoginID or password, please try again.");
                return View("Login", admin);
            }

            // Set admin session
            HttpContext.Session.SetString(nameof(admin.AdminID), admin.AdminID);
            Console.WriteLine("got here2");
            Console.WriteLine(HttpContext.Session.GetString(nameof(admin.AdminID)));
            return RedirectToAction("Index", "Home");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}