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

            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, LoginDto login)
        {
            if (id != int.Parse(login.LoginID))
                return NotFound();
            if (login.PasswordHash.Length != 64)
            {
                ModelState.AddModelError("InvalidPassword", "Invalid password hash");
            }
            if (ModelState.IsValid)
            {
                var response = JsonByAPI.ReturnResponseEditObject(Client, getLoginAPI, login);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(login);
        }
    }
}