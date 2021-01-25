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
using X.PagedList;

namespace A2.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly A2Context _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public CustomerController(A2Context context) => _context = context;
        [Route("Home")]
        public async Task<IActionResult> Home()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            return View(customer);
        }
        /// <summary>
        /// Returns the view for the ATM page as an ATMViewModel with customer of the current session.
        /// </summary>
        [Route("ATM")]
        public async Task<IActionResult> ATM()
        {
            return View(nameof(ATM), await ReturnAtmViewModel(0));
        }
        /// <summary>
        /// ATMTransaction method takes data from the ATM view and validates it. It then performs a deposit/withdraw/transfer depending
        /// on what the user selected in the form. All transactions are added to the account transactions list. Successful transactions
        /// redirect users to customer home page. Unsuccessful transactions reload the ATM page with the error displayed.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ATMTransaction(int accountNumber, string toAccountNumber, decimal amount, string transactionType, string comment)
        {


            AccountLogic processTransaction = new AccountLogic();
            var account = await _context.Account.Include(x => x.Transactions).FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

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
                    return View(nameof(ATM), ReturnAtmViewModel(amount));
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
                return View(nameof(ATM), ReturnAtmViewModel(amount));
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Home));
        }
        /// <summary>
        /// Calls customer model from database and returns an ATMviewModel of the customer with updated viewbag amount.
        /// </summary>
        /// <param name="amount">The default amount you want to show in viewbag.</param>
        private async Task<ATMViewModel> ReturnAtmViewModel(decimal amount)
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var atmViewModel = new ATMViewModel()
            {
                Customer = customer,
            };
            ViewBag.Amount = amount;
            return atmViewModel;
        }
        /// <summary>
        /// PayBill returns the view for the PayBill page as a PayBillViewModel with the Customer of the session
        /// and all available payees in the database loaded.
        /// </summary>
        public async Task<IActionResult> Transaction(int id, int? page = 1)
        {
            // handles display of transactions per page
            const int pageSize = 4;
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            // gets transactions of selected account in pagedList form
            var pagedTransactionList = await _context.Transaction.Where(X => X.AccountNumber == id).ToPagedListAsync(page, pageSize);

            var transactionsViewModel = new TransactionsViewModel()
            {
                Customer = customer,
                AccountNumber = id,
                Transactions = pagedTransactionList
            };

            return View(transactionsViewModel);
        }

        /// <summary>
        /// PayBill returns the view for the PayBill page as a PayBillViewModel with the Customer of the session
        /// and all available payees in the database loaded.
        /// </summary>
        [Route("Bills")]
        public async Task<IActionResult> PayBill()
        {
            return View(await ReturnPayBillViewModel(0));
        }
        /// <summary>
        /// Gets customer and payee information from database and returns a PayBillViewModel with this data.
        /// </summary>
        /// <param name="amount">The amount you want the viewbag to show by default.</param>
        private async Task<PayBillViewModel> ReturnPayBillViewModel(decimal amount)
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var payee = await _context.Payee.ToListAsync();
            var payBillViewModel = new PayBillViewModel()
            {
                Customer = customer,
                Payee = payee,
            };
            ViewBag.Amount = amount;
            return payBillViewModel;
        }
        /// <summary>
        /// Builds a bill model file by searching database for the ID of the bill passed in and returns the updatebill view of
        /// the specified bill
        /// </summary>
        /// <param name="id">id of the bill you want to update.</param>
        [Route("Bills/Update")]
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
                return View(nameof(PayBill), ReturnPayBillViewModel(amount));
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
        [Route("Bills/View")]
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
