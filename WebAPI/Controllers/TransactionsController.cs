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
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsManager _repo;

        public TransactionsController(TransactionsManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public List<Transaction> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public List<Transaction> Get(int id)
        {
            return _repo.GetAllByID(id);
        }

        [HttpPut]
        public void Put([FromBody] Transaction transaction)
        {
            _repo.Update(transaction.TransactionId, transaction);
        }
    }
}
