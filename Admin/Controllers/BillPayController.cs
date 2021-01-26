using Admin.Models;
using Admin.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class BillPayController : Controller
    {
        private const string getBillPayAPI = "/api/BillPays";

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var billPay = await JsonByAPI.ReturnDeserialisedObject<BillPayDto>(Client, getBillPayAPI);

            return View(billPay);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var billPay = await JsonByAPI.ReturnDeserialisedObject<BillPayDto>(Client, $"{getBillPayAPI}/{id}");

            return View(billPay);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BillPayDto billPay)
        {
            if (id != billPay.BillPayID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var response = JsonByAPI.ReturnResponseEditObject(Client, getBillPayAPI, billPay);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(billPay);
        }
    }
}
