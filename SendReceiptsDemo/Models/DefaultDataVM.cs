using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class DefaultDataVM
    {
        [Display(Name = "شرایط پیش فرض ضامنین (جداسازی با ؛ و Enter)")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? DefaultGuarantorsConditions { get; set; }

        [Display(Name = "شرایط پیش فرض مسدود کننده (جداسازی با ؛ و Enter)")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? DefaultBlockerConditions { get; set; }
    }
}
