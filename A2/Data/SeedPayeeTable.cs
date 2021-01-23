using A2.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Data
{
    public class SeedPayeeTable
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<A2Context>();

            if (context.Payee.Any())
                return;

            context.Payee.AddRange(
                new Payee
                {
                    PayeeName = "Water Company",
                    Address = "123 Fountain st",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "3000",
                    Phone = "+61 1234 1234"
                },
                new Payee
                {
                    PayeeName = "Electricity Company",
                    Address = "Some st",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "3000",
                    Phone = "+61 4321 4332"
                },
                new Payee
                {
                    PayeeName = "Test Payee",
                    Address = "123 Someother st",
                    City = "Brisbane",
                    State = "QLD",
                    PostCode = "4000",
                    Phone = "+61 2222 1111"
                });
            context.SaveChanges();
        }
    }
}
