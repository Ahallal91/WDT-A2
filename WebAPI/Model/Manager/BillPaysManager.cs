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
        /// <summary>
        /// Allows Admin get all BillPays in the system and returns them as a list.
        /// </summary>
        /// <returns>All bill pays as a list.</returns>
        public List<BillPay> GetAll()
        {
            return _context.BillPays.ToList();
        }
        /// <summary>
        /// Allows admin to get BillPays of a certain ID.
        /// </summary>
        /// <returns>Returns a list of billpays that match an ID</returns>
        public List<BillPay> GetAllByID(int id)
        {
            return _context.BillPays.Where(x => x.BillPayId == id).ToList();
        }
        /// <summary>
        /// Not currently implemented
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public List<BillPay> GetAllByIDWithDate(int id, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Allows Admin to update a billpay to
        /// block/unblock billpays and update the lockout in the database.
        /// </summary>
        /// <returns>id that was blocked or unlocked</returns>
        public int Update(int id, BillPay billPay)
        {
            _context.Update(billPay);
            _context.SaveChanges();

            return id;
        }
    }
}
