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
            TimedSwapLoginAction(login[0]);

            return RedirectToAction(nameof(Index));
        }
        private async void TimedSwapLoginAction(LoginDto login)
        {
            SwapLoginActionType(ref login);
            var response = JsonByAPI.ReturnResponseEditObject(Client, getLoginAPI, login);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("LoginEditFailed", "Unexpected error occured whilst accessing login.");
                return;
            }
            if (login.Status == ActiveType.Locked)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                SwapLoginActionType(ref login);
                JsonByAPI.ReturnResponseEditObject(Client, getLoginAPI, login);
            }
        }
        private void SwapLoginActionType(ref LoginDto login)
        {
            if (login.Status == ActiveType.Unlocked)
            {
                login.Status = ActiveType.Locked;
            }
            else
            {
                login.Status = ActiveType.Unlocked;
            }
        }
    }
}