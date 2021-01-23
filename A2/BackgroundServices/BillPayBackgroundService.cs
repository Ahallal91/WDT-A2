using A2.Controllers.BusinessObject;
using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    public class BillPayBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private AccountLogic _processTransaction;

        public BillPayBackgroundService(IServiceProvider services)
        {
            _services = services;
            _processTransaction = new AccountLogic();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        private async Task BillPayCheck(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<A2Context>();

            var account = await context.Account.ToListAsync(cancellationToken);

            foreach (var acc in account)
            {
                foreach (var bill in acc.BillPay)
                {
                    if (bill.ScheduleDate.CompareTo(DateTime.Now) > 0)
                    {
                        Account retAccount = _processTransaction.ComputeBillPay(bill, acc);
                        if (account != null)
                        {
                            
                        }
                    }
                }
            }
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
