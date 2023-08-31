using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class AddEditAccountVM
    {
        [Required]
        [Display(Name = "کد حساب")]
        public int AcountId { get; set; }
        [Display(Name = "شماره حساب")]
        [Required(ErrorMessage = "لطفا {0} شخص را وارد کنید")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "مقدار {0} باید فقط شامل اعداد باشد")]
        [Remote("isNewAccountNumber", "Admin", AdditionalFields = "AcountId")]
        public string AccountNumber { get; set; }

        [Display(Name = "کد مشتری صاحب حساب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "مقدار {0} باید حداقل 5 رقمی و فقط شامل اعداد باشد")]
        [Remote("VerifyCustomerIdForAccount", "Admin")]
        public string CustomerId { get; set; }

        [Display(Name = "امتیاز حساب")]
        public string? Score { get; set; }
    }
}
