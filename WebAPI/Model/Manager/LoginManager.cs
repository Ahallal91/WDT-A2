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

        public IEnumerable<Login> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(int id, Login item)
        {
            throw new NotImplementedException();
        }
    }
}
