using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class CheckMeetingVM
    {
        [Required(ErrorMessage ="وارد کردن پاسخ برای درخواست الزامی است")]
        public string Response { get; set; }
    }
}
