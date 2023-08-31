using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class EnterVM
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} شخص را وارد کنید")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "مقدار {0} نامعتبر است")]
        [MaxLength(11)]
        [Remote("VerifyMobileNumber", "Home")]
        public string MobileNumber { get; set; }
    }
}
