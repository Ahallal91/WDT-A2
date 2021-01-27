using Admin.Models;
using Admin.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Admin.BackgroundServices
{
    /*
     Reference: Week 8 BackgroundServiceExample - PeopleBackgroundService.cs
     */
    /// <summary>
    /// LoginStatusBackgroundService class runs as a background service checking login status of all accounts every minute.
    /// if the modifyDate of an account is past the current time and the account is status locked then it changes it to unlocked.
    /// </summary>
    public class LoginStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private const string getLoginAPI = "/api/Login";

        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public LoginStatusBackgroundService(IServiceProvider services, IHttpClientFactory clientFactory)
        {
            _services = services;
            _clientFactory = clientFactory;
        }
        /// <summary>
        /// Exceutes the LoginStatusCheck method every minute
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await LoginStatusCheck();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        /// <summary>
        /// LoginStatusCheck contacts the WebAPI for all login accoutns, checks the modifydate and status and unlocks
        /// accounts which are due to be unlocked, sending update request to API.
        /// </summary>
        private async Task LoginStatusCheck()
        {
            using var scope = _services.CreateScope();
            var login = await JsonByAPI.ReturnDeserialisedObject<LoginDto>(Client, getLoginAPI);

            for (int i = 0; i < login.Count; i++)
            {
                Console.WriteLine(login[i].LoginID);
                if (login[i].ModifyDate.CompareTo(DateTime.UtcNow) < 0 && login[i].Status == ActiveType.Locked)
                {
                    login[i].Status = ActiveType.Unlocked;
                    var response = JsonByAPI.ReturnResponseEditObject(Client, getLoginAPI, login[i]);
                }
            }
        }
    }
}
