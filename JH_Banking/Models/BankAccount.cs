using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JH_Banking.Models
{
    public class BankAccount
    {
        [Key]
        public int AccId { get; set; }
        public string AccNum { get; set; }
        [DataType(DataType.Currency)]
        public float Balance { get; set; }
        [Range(100000000000, 9999999999999999999, ErrorMessage = "must be between 12 and 19 digits")]
        public string CardNum { get; set; }
        public int UserId { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

    }
}