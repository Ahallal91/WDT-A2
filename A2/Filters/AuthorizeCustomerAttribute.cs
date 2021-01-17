using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Filters
{
    public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var custID = context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID))
            if (!custID.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
