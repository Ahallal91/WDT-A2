using A2.Models;
using A2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers
{
    /*
     * Reference: Week 7 example03 CustErrPageDemo
     */
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Displays error code pages.
        /// </summary>
        [HttpGet("/error/{errorCode}")]
        public IActionResult ErrorCode(int errorCode)
        {
            return View(new ErrorViewModel()
            {
                ErrorCode = errorCode
            });
        }
    }
}
