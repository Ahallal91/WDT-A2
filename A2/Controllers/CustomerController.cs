using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Filters;
using A2.Controllers.BusinessObject;

namespace A2.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly A2Context _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public CustomerController(A2Context context) => _context = context;
        public async Task<IActionResult> Home()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            return View(customer);
        }
        public async Task<IActionResult> ATM(int id) => View(await _context.Account.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            var account = await _context.Account.FindAsync(id);

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Deposit's must be positive.");
            }
            if (Math.Round(amount, 2) != amount)
            {
                ModelState.AddModelError(nameof(amount), "Please enter currency to 2 decimal places.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            AccountLogic processDeposit = new AccountLogic();
            account = processDeposit.Deposit(amount, account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            var account = await _context.Account.FindAsync(id);

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Deposit's must be positive.");
            }
            if (Math.Round(amount, 2) != amount)
            {
                ModelState.AddModelError(nameof(amount), "Please enter currency to 2 decimal places.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            AccountLogic processDeposit = new AccountLogic();
            account = processDeposit.Deposit(amount, account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
