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

        [HttpGet("{customerId}/{startDate}/{endDate}")]
        public List<Transaction> Get(int customerId, DateTime startDate, DateTime endDate)
        {
            return _repo.GetAllByIDWithDate(customerId, startDate, endDate);
        }
    }
}
