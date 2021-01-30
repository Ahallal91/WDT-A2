using System;
using A2.Areas.Identity.Data;
using A2.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(A2.Areas.Identity.IdentityHostingStartup))]
namespace A2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityA2Context>(options =>
                {
                    options.UseSqlServer(context.Configuration.GetConnectionString("A2Context"));

                    // Enable lazy loading.
                    options.UseLazyLoadingProxies();
                });
            });
        }
    }
}