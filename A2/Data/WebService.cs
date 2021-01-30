using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Microsoft.Extensions.DependencyInjection;
using A2.Models;
using A2.Data.JsonModels;
using A2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace A2.Data
{
    public static class WebService
    {
        /// <summary>
        /// Checks Customer and Login tables of database, if they have no data, then it populates from the
        /// JSON service link.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider interface</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IdentityA2Context>();
            const string customerConnection = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/";
            const string loginConnection = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/";
            // if any customers populated in database exit out
            if (context.Customer.Any())
            {
                return;
            }

            AddCustomerToDatabase(context, GetJson.GetJsonByURLAsync<Customers>(customerConnection, "dd/MM/yyyy hh:mm:ss tt").Result);

            // if any logins populated in database exit out
            if (context.Users.Any())
            {
                return;
            }

            AddLoginToDatabase(context, GetJson.GetJsonByURLAsync<JsonModels.Login>(loginConnection, "").Result);
        }
        /// <summary>
        /// Populates database with Customers from a JsonModels customer list.
        /// </summary>
        /// <param name="context">Context File</param>
        /// <param name="customers">Json Model customer file</param>
        private static void AddCustomerToDatabase(IdentityA2Context context, List<Customers> customers)
        {
            string tempPhone = "+61 9999 9999";
            string comment = "Opening transaction";
            string transactType = "D";
            // if there was a problem with loading the JSON file.
            if (customers == null)
            {
                return;
            }
            foreach (var cust in customers)
            {
                context.Customer.Add(new Customer()
                {
                    CustomerID = cust.CustomerID,
                    CustomerName = cust.Name,
                    Address = cust.Address,
                    City = cust.City,
                    PostCode = cust.Postcode,
                    Phone = tempPhone
                });
                foreach (var acc in cust.Accounts)
                {
                    context.Account.Add(new Models.Account()
                    {
                        AccountNumber = acc.AccountNumber,
                        AccountType = acc.AccountType,
                        CustomerID = cust.CustomerID,
                        Balance = acc.Balance,
                        ModifyDate = DateTime.Now
                    });
                    foreach (var tran in acc.Transactions)
                    {
                        context.Transaction.Add(new Models.Transaction()
                        {
                            TransactionType = transactType,
                            AccountNumber = acc.AccountNumber,
                            Amount = acc.Balance,
                            Comment = comment,
                            ModifyDate = tran.TransactionTimeUtc
                        });
                    }
                }
            }
            context.SaveChanges();
        }
        /// <summary>
        /// Populates database with logins from a JsonModels login list.
        /// </summary>
        /// <param name="context">Context File</param>
        /// <param name="logins">Json Model login file</param>
        private static void AddLoginToDatabase(IdentityA2Context context, List<JsonModels.Login> logins)
        {
            // if there was a problem with loading the JSON file.
            if (logins == null)
            {
                return;
            }

            foreach (var log in logins)
            {
                context.Users.Add(new A2User()
                {
                    Id = log.LoginID,
                    CustomerID = log.CustomerID,
                    PasswordHash = log.PasswordHash,
                    ModifyDate = DateTime.UtcNow,
                    Status = ActiveType.Unlocked
                });
            }
            context.SaveChanges();
        }
    }
}
