using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using A2.Controllers.BusinessObject;
using A2.ViewModel;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using A2.Areas.Identity.Data;

namespace A2.Controllers
{
    /// <summary>
    /// CustomerController deals with all customer role interaction such as Home, ATM, Billpay, Transactions, PayBills views.
    /// User must be of role 'Customer' to view this controllers methods.
    /// </summary>
    [Route("Customer")]
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly IdentityA2Context _context;
        private readonly UserManager<A2User> _userManager;
        private string LoginID => _userManager.GetUserId(User);
        public CustomerController(IdentityA2Context context,
            UserManager<A2User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("Home")]
        public async Task<IActionResult> Home()
        {
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == LoginID);
            return View(user.Customer);
        }
        /// <summary>
        /// Returns the view for the ATM page as an ATMViewModel with customer of the current session.
        /// </summary>
        [Route("ATM")]
        public async Task<IActionResult> ATM()
        {
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == LoginID);
            return View(nameof(ATM), new ATMViewModel()
            {
                Customer = user.Customer
            });
        }
        /// <summary>
        /// ATMTransaction method takes data from the ATM view and validates it. It then performs a deposit/withdraw/transfer using
        /// the AccountLogic class depending on what the user selected in the form. All transactions are added to the account
        /// transactions list. Successful transactions redirect users to customer home page. Unsuccessful
        /// transactions reload the ATM page with the error displayed.
        /// </summary>
        /// <param name="atmViewModel"> atmViewModel with data from the view.</param>
        [HttpPost]
        [Route("ATMTransaction")]
        public async Task<IActionResult> ATMTransaction(ATMViewModel atmViewModel)
        {
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .ThenInclude(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == LoginID);
            AccountLogic processTransaction = new AccountLogic();

            Account account = Util.Utilities.ValidateAccount(user.Customer, atmViewModel.AccountNumber);
            if (account == null)
            {
                ModelState.AddModelError("NoAccountError", "You do not own an account of that account number.");
            }
            if (atmViewModel.transfer == atmViewModel.TransactionType && ModelState.IsValid)
            {
                Account toAccount = null;
                if (!int.TryParse(atmViewModel.ToAccountNumber, out var toAccNumber))
                {
                    ModelState.AddModelError("AccountError", "You must enter an account number to transfer to.");
                }
                if (atmViewModel.AccountNumber == toAccNumber)
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
                if (ModelState.IsValid)
                {
                    account = processTransaction.Transfer(atmViewModel.Amount, account, toAccount, atmViewModel.Comment);
                }
            }
            else if (atmViewModel.deposit == atmViewModel.TransactionType && ModelState.IsValid)
            {
                account = processTransaction.Deposit(atmViewModel.Amount, account);
            }
            else if (atmViewModel.withdraw == atmViewModel.TransactionType && ModelState.IsValid)
            {
                account = processTransaction.Withdraw(atmViewModel.Amount, account);
            }
            if (account == null && ModelState.IsValid)
            {
                ModelState.AddModelError("Amount", "You have insufficient Balance for that transaction.");
            }
            if (!ModelState.IsValid)
            {
                atmViewModel.Customer = user.Customer;
                return View(nameof(ATM), atmViewModel);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Home));
        }
        /// <summary>
        /// Transaction returns a TransactionViewModel with the current session customer, their
        /// selected account via id or a default account if going from navigation bar. And a PagedList of transactions
        /// for that account. Page number is set to 1 by default if not supplied
        /// Reference: NorthwindWithPagingExample week 7
        /// </summary>
        /// <param name="id">account number id of the account to show transactions of</param>
        /// <param name="page">The pagination page for transactions</param>
        [Route("Transaction")]
        public async Task<IActionResult> Transaction(int id, int? page = 1)
        {
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .ThenInclude(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == LoginID);
            // handles display of transactions per page
            const int pageSize = 4;

            // sets default account to first account if no account id
            if (user.Customer.Accounts.Find(x => x.AccountNumber == id) == null)
            {
                id = user.Customer.Accounts.First().AccountNumber;
            }

            // gets transactions of selected account in pagedList form
            var pagedTransactionList = user.Customer.Accounts.Find(x => x.AccountNumber == id).Transactions.ToPagedList((int)page, pageSize);

            var transactionsViewModel = new TransactionsViewModel()
            {
                Customer = user.Customer,
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
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == LoginID);

            var payee = await _context.Payee.ToListAsync();
            var payBillViewModel = new PayBillViewModel()
            {
                Customer = user.Customer,
                Payee = payee,
            };
            return View(payBillViewModel);
        }

        /// <summary>
        /// AddPayBillTransaction collects data from the PayBill view and validates it, if valid it adds the specified billpay to
        /// the accounts billpay list and redirects the user to the BillPays view. If unsucessful it reloads the PayBill view
        /// displaying the error.
        /// </summary>
        [HttpPost]
        [Route("AddPayBillTransaction")]
        public async Task<IActionResult> AddPayBillTransaction(PayBillViewModel payBill)
        {
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .ThenInclude(x => x.BillPay)
                .FirstOrDefaultAsync(x => x.Id == LoginID);
            Account account = Util.Utilities.ValidateAccount(user.Customer, payBill.AccountNumber);
            if (account == null)
            {
                ModelState.AddModelError("NoAccountError", "You do not own an account of that account number.");
            }
            if (payBill.ScheduledDate.CompareTo(DateTime.Now) < 0)
            {
                ModelState.AddModelError("DateError", "You cannot schedule a date in the past.");
            }
            var payeeIDObject = await _context.Payee.FirstOrDefaultAsync(x => x.PayeeID == payBill.PayeeID);
            if (payeeIDObject == null)
            {
                ModelState.AddModelError("PayeeID", "That PayeeID does not exist.");
            }
            if (!ModelState.IsValid)
            {
                return View(nameof(PayBill), payBill);
            }
            account.BillPay.Add(new BillPay()
            {
                AccountNumber = payBill.AccountNumber,
                PayeeID = payBill.PayeeID,
                Amount = payBill.Amount,
                ScheduleDate = payBill.ScheduledDate,
                Period = payBill.Period,
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
            var user = await _context.Users
                .Include(x => x.Customer)
                .ThenInclude(x => x.Accounts)
                .ThenInclude(x => x.BillPay)
                .FirstOrDefaultAsync(x => x.Id == LoginID);
            List<BillPaysViewModel> billPaysViewModels = new List<BillPaysViewModel>();
            foreach (var acc in user.Customer.Accounts)
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
        /// <summary>
        /// Builds a bill model file by searching database for the ID of the bill passed in and returns the updatebill view of
        /// the specified bill
        /// </summary>
        /// <param name="id">id of the bill you want to update.</param>
        [Route("Bills/Update")]
        public async Task<IActionResult> UpdateBill(int id)
        {
            var updateViewModel = await ReturnUpdateViewModel(id);
            if (updateViewModel == null)
            {
                return RedirectToAction(nameof(BillPays));
            }
            return View("UpdateBill", updateViewModel);
        }
        /// <summary>
        /// Computes data from the updateBillView and updates the bill if data is valid.
        /// </summary>
        /// <param name="updateBillPays">UpdateBillPay view model</param>
        [HttpPost]
        public async Task<IActionResult> UpdatePayBillTransaction([Bind("BillPayID," +
            " AccountNumber, PayeeID, Amount, ScheduleDate, Period")] UpdateBillPayViewModel updateBillPays)
        {
            if (updateBillPays.ScheduleDate.CompareTo(DateTime.Now) < 0)
            {
                ModelState.AddModelError("DateError", "You cannot schedule a date in the past.");
            }
            var payeeIDObject = await _context.Payee.FirstOrDefaultAsync(x => x.PayeeID == updateBillPays.PayeeID);
            if (payeeIDObject == null)
            {
                ModelState.AddModelError("PayeeID", "That PayeeID does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return View("UpdateBill", ReturnUpdateViewModel(updateBillPays.BillPayID).Result);
            }
            BillPay billPay = new BillPay()
            {
                BillPayID = updateBillPays.BillPayID,
                AccountNumber = updateBillPays.AccountNumber,
                PayeeID = updateBillPays.PayeeID,
                Amount = updateBillPays.Amount,
                ScheduleDate = updateBillPays.ScheduleDate,
                Period = updateBillPays.Period,
                ModifyDate = DateTime.Now,
                Status = StatusType.Awaiting
            };
            _context.Update(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BillPays));
        }
        /// <summary>
        /// Finds the bill with specified ID and returns an UpdateBillPayViewmodel of the bill or null if it doesn't exist
        /// </summary>
        /// <param name="updateBillPays">UpdateBillPay view model</param>
        /// <returns>Populated UpdateBillPayViewModel or null if it does not exist</returns>
        private async Task<UpdateBillPayViewModel> ReturnUpdateViewModel(int id)
        {
            var bill = await _context.BillPay.FirstOrDefaultAsync(x => x.BillPayID == id);
            if (bill.Status == StatusType.Complete || bill.Status == StatusType.Blocked || bill == null)
            {
                return null;
            }
            var payee = await _context.Payee.ToListAsync();
            var updateBillPayViewModel = new UpdateBillPayViewModel()
            {
                BillPayID = bill.BillPayID,
                AccountNumber = bill.AccountNumber,
                PayeeID = bill.PayeeID,
                Amount = bill.Amount,
                ScheduleDate = bill.ScheduleDate,
                Period = bill.Period,
                ModifyDate = bill.ModifyDate,
                Status = bill.Status,
                Payees = payee
            };

            return updateBillPayViewModel;
        }
    }
}
