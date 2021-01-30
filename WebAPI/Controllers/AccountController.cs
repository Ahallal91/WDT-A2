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
        public IEnumerable<Account> Get()
        {
            return (IEnumerable<Account>)_repo.GetAll();
        }

        [HttpGet("{id}")]
        public IEnumerable<Account> Get(int id)
        {
            return _repo.GetAllByID(id);
        }

        [HttpPut]
        public void Put([FromBody] Account account)
        {
            _repo.Update(account.AccountNumber, account);
        }
    }
}
