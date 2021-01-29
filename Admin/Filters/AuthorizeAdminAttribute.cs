using Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Filters
{
    /*
     * Reference McbaExampleWithLogin AuthorizeCustomerAttribute.cs week 6
     */
    public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var adminID = context.HttpContext.Session.GetString(nameof(Administrator.AdminID));

            if (adminID == null)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
    }
}
