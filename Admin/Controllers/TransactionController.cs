using Admin.Models;
using Admin.Util;
using Admin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class TransactionController : Controller
    {
        private const string getTransactionAPI = "/api/Transactions";
        private const string getLoginAPI = "/api/Login";
        private const string getAccountAPI = "/api/Account";

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index(TransactionViewModel transactionViewModel)
        {
            var accounts = await JsonByAPI.ReturnDeserialisedObject<AccountDto>(Client, getAccountAPI);
            var transactions = await JsonByAPI.ReturnDeserialisedObject<TransactionDto>(Client, getTransactionAPI);
            if (transactionViewModel.CustomerID == 0)
            {
                ModelState.AddModelError("EmptyID", "");
            }
            var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, getLoginAPI);
            if (login.Find(x => x.CustomerID == transactionViewModel.CustomerID) == null)
            {
                ModelState.AddModelError("CustomerIDError", "That CustomerID does not exist");
            }
            // returns if customer ID is invalid
            if (!ModelState.IsValid)
            {
                return View(new TransactionViewModel()
                {
                    Accounts = accounts,
                    Transactions = transactions
                });
            }
            // filters out accounts and transactions via date.
            var accountsChosen = accounts.FindAll(x => x.CustomerID == transactionViewModel.CustomerID);

            List<TransactionDto> transactionsChosen = new List<TransactionDto>();
            for (int i = 0; i < accountsChosen.Count; i++)
            {
                transactionsChosen.AddRange(transactions.FindAll(x => x.AccountNumber == accountsChosen[i].AccountNumber
                && x.ModifyDate.CompareTo(transactionViewModel.StartDate) >= 0 &&
                x.ModifyDate.CompareTo(transactionViewModel.EndDate) <= 0));
            }

            return View(new TransactionViewModel()
            {
                CustomerID = transactionViewModel.CustomerID,
                Accounts = accountsChosen,
                Transactions = transactionsChosen,
                StartDate = transactionViewModel.StartDate,
                EndDate = transactionViewModel.EndDate
            });
        }
    }
}

