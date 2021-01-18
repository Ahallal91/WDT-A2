using System.Collections.Generic;

namespace A2.Data.JsonModels
{
    /// <summary>
    /// Account Model to load in JSON Data.
    /// </summary>
    public class Account
    {
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int CustomerID { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
