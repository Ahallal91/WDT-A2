using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministratorController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
