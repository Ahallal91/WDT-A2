using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2.Data.JsonModels
{
    /// <summary>
    /// Customers Model to load in JSON Data.
    /// </summary>
    public class Customers
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
