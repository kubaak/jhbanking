using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JH_Banking.Models
{
    public class Transaction
    {
        [Key]
        public int TranId { get; set; }
        public DateTime Date { get; set; }
        public string AccNum { get; set; }
        public string DestAccNum { get; set; }
        public string DestBankCode { get; set; }
        public DateTime DueDate { get; set; }
        [DataType(DataType.Currency)]
        public float Amount { get; set; }
    }
}