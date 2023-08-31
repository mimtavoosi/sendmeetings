using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class Right
    {
        [Key]
        public int RightId { get; set; }

        [Display(Name = "امضادار دوم ")]
        //[Required(ErrorMessage ="تعیین {0} الزامی است")]
        public int RighterId { get; set; }

        [Display(Name = "شماره حساب ")]
        //[Required(ErrorMessage = "تعیین {0} الزامی است")]
        public int AcountId { get; set; }
    }
}
