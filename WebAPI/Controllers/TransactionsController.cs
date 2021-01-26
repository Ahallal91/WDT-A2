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
    public class BillPaysController : ControllerBase
    {
        private readonly BillPaysManager _repo;

        public BillPaysController(BillPaysManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public IEnumerable<BillPay> Get(int id)
        {
            return _repo.GetAllByID(id);
        }

        [HttpPut]
        public void Put([FromBody] BillPay billpay)
        {
            _repo.Update(billpay.BillPayId, billpay);
        }
    }
}
