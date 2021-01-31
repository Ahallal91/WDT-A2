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
        /// <summary>
        /// Gets all accounts in the database.
        /// </summary>
        /// <returns>Returns a list of accounts that are available</returns>
        [HttpGet]
        public List<Account> Get()
        {
            return _repo.GetAll();
        }
        /// <summary>
        /// Gets all accounts in the database that match the customer id
        /// </summary>
        /// <returns>Returns a list of accounts that are available and that match the customer id</returns>
        [HttpGet("{customerId}")]
        public List<Account> Get(int customerId)
        {
            return _repo.GetAllByID(customerId);
        }
    }
}
