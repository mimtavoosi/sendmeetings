using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class NotificationVM
    {
        [Display(Name = "کد اعلان")]
        [Required]
        public int NotificationId { get; set; }

        [Display(Name = "عنوان اعلان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(255)]
        public string NotificationTitle { get; set; }

        [Display(Name = "وضعیت اعلان")]
        [Remote("CheckNotificationStatus", "Admin", AdditionalFields = "NotificationId")]
        public bool NotificationStatus { get; set; }

        [Display(Name = "شرح اعلان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
         public string NotificationDescription { get; set; }
    }
}
