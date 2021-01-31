using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebAPI.Model.Repository;

namespace WebAPI.Model.Manager
{
    public class BillPaysManager : IDataRepository<BillPay, int>
    {
        private readonly s3811836_a2Context _context;

        public BillPaysManager(s3811836_a2Context context)
        {
            _context = context;
        }
        public List<BillPay> GetAll()
        {
            return _context.BillPays.ToList();
        }

        public List<BillPay> GetAllByID(int id)
        {
            return _context.BillPays.Where(x => x.BillPayId == id).ToList(); ;
        }

        public int Update(int id, BillPay billPay)
        {
            _context.Update(billPay);
            _context.SaveChanges();

            return id;
        }
    }
}
