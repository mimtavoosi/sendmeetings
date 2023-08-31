using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class Admin
    {
        [Key]
        [Display(Name = "کد مدیر")]
        public int AdminId { get; set; }

        [Display(Name = "نام کاربری")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20)]
        public string Password { get; set; }

        [Display(Name = "سطح مدیریتی")]
        [MaxLength(30)]
        public string AdminType { get; set; }
    }
}
