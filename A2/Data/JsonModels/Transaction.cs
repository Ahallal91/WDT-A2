using System;

namespace A2.Data.JsonModels
{
    /// <summary>
    /// Transaction Model to load in JSON Data.
    /// </summary>
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionTimeUtc { get; set; }
    }
}
