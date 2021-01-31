using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Model.Manager;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountManager _repo;

        public AccountController(AccountManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public List<Account> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{customerId}")]
        public List<Account> Get(int customerId)
        {
            return _repo.GetAllByID(customerId);
        }
    }
}
