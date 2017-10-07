using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JH_Banking.Models
{
    public class Template
    {
        [Key]
        public int TempId { get; set; }
        public string TemplName { get; set; }
        public string AccNum { get; set; }
        public string DestAccNum { get; set; }
        public string DestBankCode { get; set; }
        [DataType(DataType.Currency)]
        public float Amount { get; set; }
        public int UserId { get; set; }
    }
}