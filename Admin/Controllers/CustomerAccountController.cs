using Admin.Filters;
using Admin.Models;
using Admin.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    [AuthorizeAdmin]
    [Route("Admin")]
    public class CustomerAccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public CustomerAccountController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;
        /// <summary>
        /// Displays all users
        /// </summary>
        [Route("Logins")]
        public async Task<IActionResult> Index()
        {
            var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, APIUrl.GetLoginAPI);

            return View(login);
        }
        /// <summary>
        /// Deals with locking and unlocking of user accounts. LoginStatusBackgroundService will unlock accounts after 1 minute has passed.
        /// </summary>
        [Route("LoginEdit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, $"{APIUrl.GetLoginAPI}/{id}");
            if (!login.Any())
            {
                return NotFound();
            }
            login[0] = SwapLoginActionType(login[0]);
            var response = JsonByAPI.ReturnResponseEditObject(Client, APIUrl.GetLoginAPI, login[0]);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("LoginEditFailed", "Unexpected error occured whilst accessing login.");
                return View(login);
            }

            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Sets the status of a login to unlocked if it was locked or locked if it was unlocked
        /// </summary>
        /// <returns>The edited login</returns>
        private static LoginDto SwapLoginActionType(LoginDto login)
        {
            if (login.Status == ActiveType.Unlocked)
            {
                login.Status = ActiveType.Locked;
                login.ModifyDate = DateTime.UtcNow.AddMinutes(1);
            }
            else
            {
                login.Status = ActiveType.Unlocked;
            }
            return login;
        }
    }
}
