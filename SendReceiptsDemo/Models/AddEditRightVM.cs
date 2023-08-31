using Microsoft.AspNetCore.Mvc;
using SendReceiptsDemo.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class AddEditRightVM
    {
        [Required]
        public int RightId { get; set; }

        [Display(Name = "کد مشتری امضادار دوم")]
        [Required(ErrorMessage = "تعیین {0} الزامی است")]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "مقدار {0} باید حداقل 5 رقمی و فقط شامل اعداد باشد")]
        [Remote("VerifyCustomerId1", "Admin")]
        public string RighterCustomerId { get; set; }

        [Display(Name = "شماره حساب ")]
        [Required(ErrorMessage = "تعیین {0} الزامی است")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "مقدار {0} باید فقط شامل اعداد باشد")]
        [Remote("VerifyAccountForRight", "Admin",AdditionalFields = "RighterCustomerId")]
        public string AccountNumber { get; set; } 
  }
}
