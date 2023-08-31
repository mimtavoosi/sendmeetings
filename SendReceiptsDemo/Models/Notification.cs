using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [Display(Name = "عنوان اعلان")]
        [MaxLength(255)]
        public string NotificationTitle { get; set; }

        [Required]
        [Display(Name = "وضعیت اعلان")]
        [MaxLength(50)]
        public string NotificationStatus { get; set; }

        [Required]
        [Display(Name = "شرح اعلان")]
        public string NotificationDescription { get; set; }
    }
}
