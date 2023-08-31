using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class BlockReqVM
    {
        public int MeetId { get; set; }
        public int ReqIndex { get; set; }
        public string ReqName { get; set; }
        public string ReqAccountNumber { get; set; }
        public string ScorerAccountNumber { get; set; }

        [Required(ErrorMessage = "لطفا امتیاز درخواستی را وارد کنید")]
        [Remote("ChangeBlockReqScore", "Home", AdditionalFields = "MeetId")]
        public string BlockReqScore { get; set; }

        [Display(Name = "شرایط ضامنین")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? GuarantorsConditions { get; set; }

        [Display(Name = "شرایط مسدود کننده")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? BlockerConditions { get; set; }

        [Display(Name = "مدت بازپرداخت (روز)")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int RepaymentPeriod { get; set; }
    }
}
