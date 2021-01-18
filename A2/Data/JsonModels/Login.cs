using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2.Data.JsonModels
{
    /// <summary>
    /// Login Model to load in JSON Data.
    /// </summary>
    public class Login
    {
        public string LoginID { get; set; }
        public int CustomerID { get; set; }
        public string PasswordHash { get; set; }
    }
}
