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

        public IEnumerable<AspNetUser> GetAll()
        {
            return _context.AspNetUsers.ToList();
        }

        public IEnumerable<AspNetUser> GetAllByID(int id)
        {
            return _context.AspNetUsers.ToList().Where(x => x.Id == id.ToString());
        }

        public int Update(int id, AspNetUser login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
