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
        /// <summary>
        /// Returns the view for the ATM page as an ATMViewModel with customer of the current session.
        /// </summary>
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
        /// <summary>
        /// ATMTransaction method takes data from the ATM view and validates it. It then performs a deposit/withdraw/transfer depending
        /// on what the user selected in the form. All transactions are added to the account transactions list. Successful transactions
        /// redirect users to customer home page. Unsuccessful transactions reload the ATM page with the error displayed.
        /// </summary>
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
                return View(nameof(ATM), atmViewModel);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Home));
        }
        public async Task<IActionResult> Transaction(int id) => View(await _context.Account.FindAsync(id));
        /// <summary>
        /// PayBill returns the view for the PayBill page as a PayBillViewModel with the Customer of the session
        /// and all available payees in the database loaded.
        /// </summary>
        public async Task<IActionResult> PayBill()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var payee = await _context.Payee.ToListAsync();
            var payBillViewModel = new PayBillViewModel()
            {
                Customer = customer,
                Payee = payee
            };
            return View(payBillViewModel);
        }

        public async Task<IActionResult> UpdateBill(int id)
        {
            var bill = await _context.BillPay.FirstOrDefaultAsync(x => x.BillPayID == id);

            return View("UpdateBill", bill);
        }

        /// <summary>
        /// AddPayBillTransaction collects data from the PayBill view and validates it, if valid it adds the specified billpay to
        /// the accounts billpay list and redirects the user to the BillPays view. If unsucessful it reloads the PayBill view
        /// displaying the error.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddPayBillTransaction(int accountNumber, int payeeID, decimal amount, DateTime scheduledDate, string period)
        {
            if (scheduledDate.CompareTo(DateTime.Now) < 0)
            {
                ModelState.AddModelError("DateError", "You cannot schedule a date in the past.");
            }
            var payeeIDObject = await _context.Payee.FindAsync(payeeID);
            if (payeeIDObject == null)
            {
                ModelState.AddModelError("PayeeID", "That PayeeID does not exist.");
            }
            if (!ModelState.IsValid)
            {
                var customer = await _context.Customer.Include(x => x.Accounts).
                    FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
                var payBillViewModel = new PayBillViewModel()
                {
                    Customer = customer,
                };
                ViewBag.Amount = amount;
                return View(nameof(PayBill), payBillViewModel);
            }
            var account = await _context.Account.FindAsync(accountNumber);
            account.BillPay.Add(new BillPay()
            {
                AccountNumber = accountNumber,
                PayeeID = payeeID,
                Amount = amount,
                ScheduleDate = scheduledDate,
                Period = period,
                ModifyDate = DateTime.Now,
                Status = StatusType.Awaiting
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BillPays));
        }
        /// <summary>
        /// BillPays returns a view of BillPayViewModel as a list with Customer data, accountnumber information payee information and
        /// BillPay information. This is used to display all the bills to the user so they can see billpay information. The view for this
        /// page should refresh automatically to inform user in real-time when the background service updates bills.
        /// </summary>
        public async Task<IActionResult> BillPays()
        {
            var customer = await _context.Customer.FindAsync(CustomerID);
            List<BillPaysViewModel> billPaysViewModels = new List<BillPaysViewModel>();
            foreach (var acc in customer.Accounts)
            {
                foreach (var bill in acc.BillPay)
                {
                    billPaysViewModels.Add(new BillPaysViewModel()
                    {
                        BillPayID = bill.BillPayID,
                        AccountNumber = acc.AccountNumber,
                        PayeeID = bill.PayeeID,
                        PayeeName = bill.Payee.PayeeName,
                        Amount = bill.Amount,
                        ScheduleDate = bill.ScheduleDate,
                        Period = bill.Period,
                        Status = bill.Status
                    });
                }
            }

            return View(billPaysViewModels);
        }
    }
}
