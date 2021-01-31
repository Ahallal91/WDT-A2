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
    public class BillPayController : ControllerBase
    {
        private readonly BillPaysManager _repo;

        public BillPayController(BillPaysManager repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// Gets all Billpays available
        /// </summary>
        /// <returns>returns a list of all billpays which are available</returns>
        [HttpGet]
        public List<BillPay> Get()
        {
            return _repo.GetAll();
        }
        /// <summary>
        /// Gets all Billpays available that match the billpay id
        /// </summary>
        /// <returns>returns a list of all billpays which are available and match the billpay id</returns>
        [HttpGet("{id}")]
        public List<BillPay> Get(int id)
        {
            return _repo.GetAllByID(id);
        }
        /// <summary>
        /// Updates a billpay
        /// </summary>
        /// <returns>Updates the billpay object passed in</returns>
        [HttpPut]
        public void Put([FromBody] BillPay billpay)
        {
            _repo.Update(billpay.BillPayId, billpay);
        }
    }
}
