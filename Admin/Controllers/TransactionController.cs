using Admin.Filters;
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
using X.PagedList;

namespace Admin.Controllers
{
    [Route("Admin")]
    [AuthorizeAdmin]
    public class TransactionController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        [Route("Transactions")]
        public async Task<IActionResult> Index(TransactionViewModel transactionViewModel, int? page = 1)
        {
            const int pageSize = 6;
            var accounts = await JsonByAPI.ReturnDeserialisedObject<AccountDto>(Client, APIUrl.GetAccountAPI);
            var transactions = await JsonByAPI.ReturnDeserialisedObject<TransactionDto>(Client, APIUrl.GetTransactionAPI);
            if (transactionViewModel.CustomerID == 0)
            {
                ModelState.AddModelError("EmptyID", "");
            }
            if (ModelState.IsValid)
            {
                var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, APIUrl.GetLoginAPI);
                if (login.Find(x => x.CustomerId == transactionViewModel.CustomerID) == null)
                {
                    ModelState.AddModelError("CustomerIDError", "That CustomerID does not exist");
                }
            }
            // returns if customer ID is invalid
            if (!ModelState.IsValid)
            {
                return View(new TransactionViewModel()
                {
                    CustomerID = transactionViewModel.CustomerID,
                    Accounts = accounts,
                    Transactions = transactions.ToPagedList((int)page, pageSize),
                    StartDate = transactionViewModel.StartDate,
                    EndDate = transactionViewModel.EndDate
                });
            }
            // filters out accounts and transactions via date and customerID
            var accountsChosen = await JsonByAPI.ReturnDeserialisedObject<AccountDto>(Client,
                $"{APIUrl.GetAccountAPI}/{transactionViewModel.CustomerID}");
            var transactionsChosen = await JsonByAPI.ReturnDeserialisedObject<TransactionDto>(Client,
                $"{APIUrl.GetTransactionAPI}/{transactionViewModel.CustomerID}" +
                $"/{transactionViewModel.StartDate.ToString("yyyy-MM-dd")}/" +
                $"{transactionViewModel.EndDate.ToString("yyyy-MM-dd")}");

            return View(new TransactionViewModel()
            {
                CustomerID = transactionViewModel.CustomerID,
                Accounts = accountsChosen,
                Transactions = transactionsChosen.ToPagedList((int)page, pageSize),
                StartDate = transactionViewModel.StartDate,
                EndDate = transactionViewModel.EndDate
            });
        }
    }
}

