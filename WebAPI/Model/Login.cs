using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Model
{
    public partial class Login
    {
        public string LoginId { get; set; }
        public int CustomerId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Status { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
