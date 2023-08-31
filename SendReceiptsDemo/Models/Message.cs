using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendReceiptsDemo.Models
{
    public class Message
    {
        [Key]
        [Display(Name = "کد پیامک")]
        public int MessageId { get; set; }

        [Display(Name = "نام مشتری")]
        public int? CustomerId { get; set; }

        [Display(Name = "متن پیامک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800)]
        public string MessageText { get; set; }

        [Display(Name = "وضعیت ارسال")]
        [MaxLength(800)]
        public string? SentState { get; set; }


        [Display(Name = "تاریخ ارسال")]
        [MaxLength(30)]
        public string? SentDate { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
    }
}