using A2.Controllers.BusinessObject;
using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace A2.BackgroundServices
{
    /*
     Reference: Week 8 BackgroundServiceExample - PeopleBackgroundService.cs
     */
    /// <summary>
    /// BillPayBackgroundService class runs as a background service checking billpay of accounts every 30 seconds.
    /// </summary>
    public class BillPayBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private AccountLogic _processTransaction;

        public BillPayBackgroundService(IServiceProvider services)
        {
            _services = services;
            _processTransaction = new AccountLogic();
        }
        /// <summary>
        /// Exceutes the billpaycheck method every 30 seconds
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await BillPayCheck(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        /// <summary>
        /// BillPayCheck pulls Accounts from database and iterates over every account in the system. It checks their
        /// billpays to see if any awaiting bills have a scheduleDate which has past. It calls ComputeBillPay in AccountLogic class
        /// to process the bill. Completed bills status is changed to Completed, if there is insufficient funds they are set to
        /// Rejected.
        /// </summary>
        private async Task BillPayCheck(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<A2Context>();
            var account = await context.Account.ToListAsync(stoppingToken);

            for (int i = 0; i < account.Count; i++)
            {
                for (int ii = 0; ii < account[i].BillPay.Count; ii++)
                {
                    if (account[i].BillPay[ii].ScheduleDate.CompareTo(DateTime.UtcNow) < 0 && account[i].BillPay[ii].Status == StatusType.Awaiting)
                    {
                        account[i] = _processTransaction.ComputeBillPay(account[i].BillPay[ii], account[i]);
                    }
                }
            }
            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
