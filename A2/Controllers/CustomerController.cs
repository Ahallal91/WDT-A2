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
using Utilities;
using A2.Controllers.BusinessObject;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using A2.ViewModel;

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
            var atmViewModel = new ATMViewModel()
            {
                Customer = customer,
            };
            return View(atmViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ATMTransaction(int accountNumber, string toAccountNumber, decimal amount, string transactionType, string comment)
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            AccountLogic processTransaction = new AccountLogic();
            var account = await _context.Account.FindAsync(accountNumber);
            Account toAccount = null;
            if (transactionType == nameof(processTransaction.Transfer))
            {
                if (!int.TryParse(toAccountNumber, out var toAccNumber))
                {
                    ModelState.AddModelError("AccountError", "You must enter an account number to transfer to.");
                }
                if (accountNumber == toAccNumber)
                {
                    ModelState.AddModelError("AccountError", "You cannot transfer to your own account.");
                }
                else if (Validator.FourDigits(toAccNumber))
                {
                    toAccount = await _context.Account.FindAsync(toAccNumber);
                    if (toAccount == null)
                    {
                        ModelState.AddModelError("AccountError", "The account you are transferring to does not exist");
                    }
                }
                if (!ModelState.IsValid)
                {
                    var atmViewModel = new ATMViewModel()
                    {
                        Customer = customer,
                    };
                    ViewBag.Amount = amount;
                    return View("ATM", atmViewModel);
                }
                account = processTransaction.Transfer(amount, account, toAccount, comment);
            }
            else if (transactionType == nameof(processTransaction.Deposit))
            {
                account = processTransaction.Deposit(amount, account);
            }
            else if (transactionType == nameof(processTransaction.Withdraw))
            {
                account = processTransaction.Withdraw(amount, account);
            }
            if (account == null)
            {
                ModelState.AddModelError("Amount", "You have insufficient Balance for that transaction.");
                var atmViewModel = new ATMViewModel()
                {
                    Customer = customer,
                };
                ViewBag.Amount = amount;
                return View("ATM", atmViewModel);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Home));
        }
        public async Task<IActionResult> Transaction(int id) => View(await _context.Account.FindAsync(id));
        public async Task<IActionResult> PayBill()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var payBillViewModel = new PayBillViewModel()
            {
                Customer = customer,
            };
            return View(payBillViewModel);
        }
    }
}
