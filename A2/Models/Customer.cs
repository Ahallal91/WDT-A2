using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 * Reference McbaExampleWithLogin Customer.cs week 6
 */
namespace A2.Models
{
    /// <summary>
    /// Customer Record - Please see Readme for full justification of the Customer model as a record. 
    /// As Customer model is consistently passed around the program, the use of a record in this case
    /// helps reduce resources used when using the Customer model. There is also little need to modify 
    /// this model unless the customer is updating their contact information. Therefore it makes sense to 
    /// have this class as a record rather than a normal class.
    /// </summary>
    public class Customer
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "CustomerID must be 4 digits long.")]
        public int CustomerID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Enter a valid name")]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [RegularExpression("[0-9]\\d{9}", ErrorMessage = "Enter a valid 9 digit TFN.")]
        [StringLength(11)]
        public string TFN { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = "Enter a valid city")]
        public string City { get; set; }
        [RegularExpression(@"^(?-i:NSW|QLD|SA|TAS|VIC)?$", ErrorMessage = "Enter a valid State.")]
        [StringLength(20)]
        public string State { get; set; }
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Enter a valid 4 digit Postcode.")]
        [StringLength(10)]
        public string PostCode { get; set; }
        [Required]
        [RegularExpression(@"^[+]?(61)\s\d{4}\s\d{4}$", ErrorMessage = "Enter a valid phone number.")]
        [StringLength(15)]
        public string Phone { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
}