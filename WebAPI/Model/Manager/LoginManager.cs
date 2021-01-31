using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model.Repository;

namespace WebAPI.Model.Manager
{
    public class LoginManager : IDataRepository<AspNetUser, int>
    {
        private readonly s3811836_a2Context _context;

        public LoginManager(s3811836_a2Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Allows Admin project to get all user logins available in the database
        /// and returns them as a list.
        /// </summary>
        /// <returns>A List of AspNetUsers (logins) </returns>
        public List<AspNetUser> GetAll()
        {
            return _context.AspNetUsers.ToList();
        }
        /// <summary>
        /// Allows Admin project to get all user logins by their id and return them as a list.
        /// </summary>
        /// <returns>A List of AspNetUsers (logins) that match the id</returns>
        public List<AspNetUser> GetAllByID(int id)
        {

            return _context.AspNetUsers.Where(x => x.Id == id.ToString()).ToList();
        }
        /// <summary>
        /// Currently not implemented in admin project so not implemented here.
        /// </summary>
        public List<AspNetUser> GetAllByIDWithDate(int id, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Allows Admin to update a login user to
        /// lock/unlock user logins and update the lockout in the database.
        /// </summary>
        /// <returns>id that was locked or unlocked</returns>
        public int Update(int id, AspNetUser login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
