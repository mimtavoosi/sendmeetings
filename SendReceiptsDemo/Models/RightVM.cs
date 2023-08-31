using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class RightVM
    {
        [Required]
        public int RightId { get; set; }

        [Display(Name = "شماره حساب ")]
        //[Required(ErrorMessage = "تعیین {0} الزامی است")]
        public string AccountNumber { get; set; }

        [Display(Name = "امضادار دوم ")]
        //[Required(ErrorMessage = "تعیین {0} الزامی است")]
        public string RighterName { get; set; }

        [Display(Name = "صاحب حساب اصلی ")]
        //[Required(ErrorMessage = "تعیین {0} الزامی است")]
        public string RightOwnerName { get; set; }
    }
}
