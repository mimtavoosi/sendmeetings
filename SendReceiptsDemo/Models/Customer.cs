using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "کد مشتری")]
        public int CustomerId { get; set; }

        [Display(Name ="کد ملی")]
        //[Required(ErrorMessage ="لطفا {0} شخص را وارد کنید")]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "مقدار {0} باید 10 رقمی و فقط شامل اعداد باشد")]
        //[Remote("isNewNationalCode", "Admin",AdditionalFields = "CustomerId")]
        public string? NationalCode { get; set; }

        [Display(Name = "شماره موبایل")]
        //[Required(ErrorMessage = "لطفا {0} شخص را وارد کنید")]
        //[RegularExpression(@"^([0-9]{11})$", ErrorMessage = "مقدار {0} باید 11 رقمی و فقط شامل اعداد باشد")]
        //[Remote("isNewMobileNumber", "Admin", AdditionalFields = "CustomerId")]
        public string MobileNumber { get; set; }

        [Display(Name = "نام")]
        //[Required(ErrorMessage = "لطفا {0} شخص را وارد کنید")]
        public string? FullName { get; set; }

        [Display(Name = "نام پدر")]
        //[Required(ErrorMessage = "لطفا {0} شخص را وارد کنید")]
        public string? FatherName { get; set; }

        [Display(Name = "شناسه واریز")]
        //[RegularExpression(@"^[0-9]*$", ErrorMessage = "مقدار {0} باید فقط شامل اعداد باشد")]
        public string? VarizId { get; set; }

        [Display(Name = "توضیحات")]
        public string? CustomerDescription { get; set; }

        public List<BankAccount>? BankAccounts { get; set; }

        public List<Meeting>? Meetings { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
