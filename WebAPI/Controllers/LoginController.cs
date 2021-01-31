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
        /// <summary>
        /// Gets all available user logins in the system
        /// </summary>
        /// <returns>Returns all available user logins as a list</returns>
        [HttpGet]
        public List<AspNetUser> Get()
        {
            return _repo.GetAll();
        }
        /// <summary>
        /// Gets all available user logins in the system that match the user id
        /// </summary>
        /// <returns>Returns all available user logins as a list that match the user id</returns>
        [HttpGet("{id}")]
        public List<AspNetUser> Get(int id)
        {
            return _repo.GetAllByID(id);
        }
        /// <summary>
        /// Updates the login passed in.
        /// </summary>
        [HttpPut]
        public void Put([FromBody] AspNetUser login)
        {
            _repo.Update(int.Parse(login.Id), login);
        }
    }
}
