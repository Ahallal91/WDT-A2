using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Model.Repository;

namespace WebAPI.Model.Manager
{
    public class LoginManager : IDataRepository<Login, int>
    {
        private readonly s3811836_a2Context _context;

        public LoginManager(s3811836_a2Context context)
        {
            _context = context;
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Logins.ToList();
        }

        public IEnumerable<Login> GetAllByID(int id)
        {
            return _context.Logins.ToList().Where(x => x.LoginId == id.ToString());
        }

        public int Update(int id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
