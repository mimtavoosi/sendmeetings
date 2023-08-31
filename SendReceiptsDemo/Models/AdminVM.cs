using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class AdminVM
    {
        [Required]
        [Display(Name = "کد مدیر")]
        public int AdminId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20)]
        [RegularExpression(@"^(?=.{4,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "نام کاربری نامعتبر است")]
        [Remote("isNewUserName", "Admin", AdditionalFields = "AdminId ")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20)]
        [DataType(DataType.Password)] //Hide Characters
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")] //check exist number & alphabet chars in password field
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)] //Hide Characters
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار کلمه عبور یکسان نیستند")] //Compare value with Password field
        [MaxLength(20)]
        public string RePassword { get; set; }

        [Display(Name = "سطح مدیریتی")]
        public string? AdminType { get; set; }
    }
}
