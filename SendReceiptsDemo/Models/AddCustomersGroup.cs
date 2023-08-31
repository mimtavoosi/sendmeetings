using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class AddCustomersGroup
    {
        [Display(Name = "فایل اکسل اطلاعات مشتریان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile CustomersFile { get; set; }

    }
}
