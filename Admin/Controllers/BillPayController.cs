using Admin.Models;
using Admin.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using X.PagedList;

namespace Admin.Controllers
{
    public class BillPayController : Controller
    {
        private const string getBillPayAPI = "/api/BillPay";

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index(int? page = 1)
        {
            const int pageSize = 6;
            var billPay = await JsonByAPI.ReturnDeserialisedObject<BillPayDto>(Client, getBillPayAPI);

            return View(billPay.ToPagedList((int)page, pageSize));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var billPay = await JsonByAPI.ReturnDeserialisedObject<BillPayDto>(Client, $"{getBillPayAPI}/{id}");
            if (!billPay.Any())
            {
                return NotFound();
            }
            billPay[0] = SwapBillActionType(billPay[0]);
            var response = JsonByAPI.ReturnResponseEditObject(Client, getBillPayAPI, billPay[0]);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("BillEditFailed", "Unexpected error occured whilst accessing Billpay.");
                return View(billPay);
            }

            return RedirectToAction(nameof(Index));
        }
        private BillPayDto SwapBillActionType(BillPayDto billPay)
        {
            if (billPay.Status == StatusType.Awaiting)
            {
                billPay.Status = StatusType.Blocked;
            }
            else
            {
                billPay.Status = StatusType.Awaiting;
            }
            return billPay;
        }
    }
}
