using Admin.Models;
using Admin.Util;
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

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var transactions = await JsonByAPI.ReturnDeserialisedObject<TransactionDto>(Client, getTransactionAPI);

            return View(transactions);
        }

        public async Task<IActionResult> Filter(int? id)
        {
            if (id == null)
                return NotFound();

            var transactions = await JsonByAPI.ReturnDeserialisedObject<TransactionDto>(Client, $"{getTransactionAPI}/{id}");

            return View(transactions);
        }
    }
}

