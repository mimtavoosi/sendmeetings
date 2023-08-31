using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class GetMeetingVM
    {

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Remote("CheckAmount", "Home")]
        public string Amount { get; set; }

        [Display(Name = "تعداد اقساط")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Count { get; set; }

        [Display(Name = "شغل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string Job { get; set; }

        [Display(Name = "نشانی محل کار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "معرف")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string? Reagent { get; set; }

        [Display(Name = "توضیحات")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Description { get; set; }
        public bool Loan { get; set; }

        [Display(Name = "شماره حساب امتیاز دهنده")]
        [Remote("CheckReqAccount", "Home", AdditionalFields = "Loan")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(13)]
        public string? ScorerAccountNumber { get; set; }

        [Display(Name = "امتیاز درخواستی")]
        [Remote("CheckReqScore", "Home", AdditionalFields = "Loan,Amount")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? ReqScore { get; set; }

		public bool BlockRequest { get; set; }

		[Display(Name = "شماره حساب امتیاز دهنده")]
		[Remote("CheckBlockAccount", "Home", AdditionalFields = "BlockRequest")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(13)]
		public string? BlockScorerAccountNumber { get; set; }

		[Display(Name = "امتیاز درخواستی")]
		[Remote("CheckBlockScore", "Home", AdditionalFields = "BlockRequest,Amount")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public string? BlockReqScore { get; set; }

	}
}
