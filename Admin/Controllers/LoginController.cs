using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Admin.Util;

namespace Admin.Controllers
{
    public class LoginController : Controller
    {
        private const string getLoginAPI = "/api/Login";

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public LoginController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, getLoginAPI);

            return View(login);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, $"{getLoginAPI}/{id}");
            if (!login.Any())
            {
                return NotFound();
            }
            login[0] = SwapLoginActionType(login[0]);
            var response = JsonByAPI.ReturnResponseEditObject(Client, getLoginAPI, login[0]);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("LoginEditFailed", "Unexpected error occured whilst accessing login.");
                return View(login);
            }

            return RedirectToAction(nameof(Index));
        }
        private LoginDto SwapLoginActionType(LoginDto login)
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