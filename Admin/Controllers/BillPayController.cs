using Admin.Filters;
using Admin.Models;
using Admin.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using X.PagedList;

namespace Admin.Controllers
{
    [AuthorizeAdmin]
    [Route("Admin")]
    public class BillPayController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;
        /// <summary>
        /// Returns all the bill pays as a pagedList
        /// </summary>
        [Route("BillPays")]
        public async Task<IActionResult> Index(int? page = 1)
        {
            const int pageSize = 6;
            var billPay = await JsonByAPI.ReturnDeserialisedObject<BillPayDto>(Client, APIUrl.GetBillPayAPI);

            return View(billPay.ToPagedList((int)page, pageSize));
        }
        /// <summary>
        /// Allows Admin to block and unblock billpays and returns the admin to the view.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var billPay = await JsonByAPI.ReturnDeserialisedObject<BillPayDto>(Client, $"{APIUrl.GetBillPayAPI}/{id}");
            if (!billPay.Any())
            {
                return NotFound();
            }
            billPay[0] = SwapBillActionType(billPay[0]);
            var response = JsonByAPI.ReturnResponseEditObject(Client, APIUrl.GetBillPayAPI, billPay[0]);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("BillEditFailed", "Unexpected error occured whilst accessing Billpay.");
                return View(billPay);
            }

            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Sets a billpay to awaiting status if it was blocked or blocked if it was awaiting
        /// </summary>
        /// <returns>The billpay that was updated</returns>
        private static BillPayDto SwapBillActionType(BillPayDto billPay)
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
