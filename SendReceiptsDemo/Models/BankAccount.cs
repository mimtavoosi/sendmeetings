using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendReceiptsDemo.Models
{
    public class BankAccount
    {
        [Key]
        [Display(Name = "کد حساب")]
        public int AcountId { get; set; }

        [Display(Name = "شماره حساب")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AccountNumber { get; set; }

        [Display(Name = "امتیاز")]
        public string Score { get; set; }

        [Display(Name = "صاحب حساب")]
        //[Required(ErrorMessage = "لطفا {0} را از لیست انتخاب کنید")]
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")] 
        public Customer? Customer { get; set; }
        public List<Meeting>? Meetings { get; set; }
    }
}
