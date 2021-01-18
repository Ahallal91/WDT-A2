using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly A2Context _context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(A2Context context) => _context = context;
        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            return View(customer);
        }
    }
}
