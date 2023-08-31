using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendReceiptsDemo.Models
{
    public class SelectAccountVM
    {
        [Required(ErrorMessage ="لطفا یک {0} را انتخاب کنید")]
        [Display(Name = "حساب")]
        public int? BankAcountId { get; set; }
    }
}
