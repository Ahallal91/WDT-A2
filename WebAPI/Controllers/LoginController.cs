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
    public class LoginController : ControllerBase
    {
        private readonly LoginManager _repo;

        public LoginController(LoginManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public IEnumerable<Login> Get(int id)
        {
            return _repo.GetAllByID(id);
        }

        [HttpPut]
        public void Put([FromBody] Login login)
        {
            _repo.Update(int.Parse(login.LoginId), login);
        }
    }
}
