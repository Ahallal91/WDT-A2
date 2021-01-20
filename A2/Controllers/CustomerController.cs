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
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public async Task<IActionResult> ATM()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            return View(customer);
        }

        public async Task<IActionResult> Transaction(int id) => View(await _context.Account.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> ATMTransaction(int accountNumber, int toAccountNumber, decimal amount, string transactionType, string comment)
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            if (amount <= 0)
            {
                ModelState.AddModelError("Amount", "Amount must be positive");
            }
            if (Math.Round(amount, 2) != amount)
            {
                ModelState.AddModelError("Amount", "Please enter currency to 2 decimal places.");
            }
            if (accountNumber == toAccountNumber)
            {
                ModelState.AddModelError("AccountError", "You cannot transfer to your own account.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View("ATM", customer);
            }

            AccountLogic processTransaction = new AccountLogic();
            var account = await _context.Account.FindAsync(accountNumber);
            if (transactionType == nameof(Transfer) && toAccountNumber != 0)
            {
                var toAccount = await _context.Account.FindAsync(toAccountNumber);
                if (toAccount == null)
                {
                    ModelState.AddModelError("AccountError", "The account you are transferring to does not exist");
                    ViewBag.Amount = amount;
                    return View("ATM", customer);
                }
                account = processTransaction.Transfer(amount, account, toAccountNumber, comment);
            }
            else if (transactionType == nameof(Deposit))
            {
                account = processTransaction.Deposit(amount, account);
            }
            else if (transactionType == nameof(Withdraw))
            {
                account = processTransaction.Withdraw(amount, account);
            }
            if (account == null)
            {
                ModelState.AddModelError("Amount", "You have insufficient Balance for that transaction.");
                ViewBag.Amount = amount;
                return View("ATM", customer);
            }
            return RedirectToAction(nameof(Home));
        }
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            var account = await _context.Account.FindAsync(id);

            AccountLogic processDeposit = new AccountLogic();
            account = processDeposit.Deposit(amount, account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Home));
        }
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            var account = await _context.Account.FindAsync(id);
            var customer = await _context.Customer.Include(x => x.Accounts).
    FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            AccountLogic processWithdraw = new AccountLogic();
            account = processWithdraw.Withdraw(amount, account);
            if (account == null)
            {
                ModelState.AddModelError("Amount", "You have insufficient Balance to Withdraw that amount.");
                return View();
            }
            else
            {
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Home));
        }
        public async Task<IActionResult> Transfer(int id, decimal amount, int toAccountNumber)
        {
            return RedirectToAction(nameof(Home));
        }
    }
}
