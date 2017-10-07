using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JH_Banking.Models
{
    public interface IUserAccount
    {
       string Email{get;set;}
    }
    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        //[Required(ErrorMessage = "Firstname is required.")]
        public string FirstName { get; set;}

        [Required(ErrorMessage = "Lastname is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Person Identification Number is required.")]

        [RegularExpression("[0-9]{2}[0,1,5][0-9][0-9]{2}/?[0-9]{4}",ErrorMessage = "Invalid PID")]
        public string PidNum { get; set; }
        
        [Required(ErrorMessage = "Place of birth is required.")]
        public string PlaceOfBirth { get; set; }

        [Required(ErrorMessage = "Citizenship is required.")]
        public string Citizenship { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Number of descriptive is required")]
        public string NumberOfDescriptive { get; set; }
        [Required(ErrorMessage = "Town is required.")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Zip is required.")]
        [RegularExpression(@"\d{5}-?(\d{4})?$", ErrorMessage = "Please enter valid ZIP code.")]
        public string Zip { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }
        [RegularExpression(@"^((\+)?)([\s-.\(\)]*\d{1}){8,13}$", ErrorMessage = "Invalid phone.")]
        public string Phone { get; set; }

        public string MailStreet { get; set; }
        public string MailNumberOfDescriptive { get; set; }
        public string MailTown { get; set; }
        [RegularExpression(@"\d{5}-?(\d{4})?$", ErrorMessage = "Please enter valid ZIP code.")]
        public string MailZip { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Admin { get; set; }

        public virtual List<BankAccount> BankAccounts { get; set; }
        public virtual List<Template> Templates { get; set; }
    }

}