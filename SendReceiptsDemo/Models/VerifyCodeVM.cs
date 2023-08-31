using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class VerifyCodeVM
    {
        [Display(Name = "کد ارسال شده را وارد کنید")]
        [Required(ErrorMessage = "لطفا {0}")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "کد ارسال شده نامعتبر است")]
        [MaxLength(6)]
        //[Remote("VerifySendedCode", "Home")]
        public string VerifyCode { get; set; }
    }
}
