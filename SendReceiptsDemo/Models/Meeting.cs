using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendReceiptsDemo.Models
{
    public class Meeting
    {
        [Key]
        [Display(Name = "کد پیگیری")]
        public int MeetingId { get; set; }

        [Display(Name = "مبلغ")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Amount { get; set; }

        [Display(Name = "شماره حساب")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(13)]
        public string AccountNumber { get; set; }

        [Display(Name = "تعداد اقساط")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Count { get; set; }

        [Display(Name = "شغل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string Job { get; set; }

        [Display(Name = "نشانی محل کار")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "معرف")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string? Reagent { get; set; }

        [Display(Name = "توضیحات")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Description { get; set; }

		[Display(Name = "پاسخ")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public string Response { get; set; }

		[Display(Name = "وضعیت")]
		[MaxLength(300)]
		public string Status { get; set; }

		[Display(Name = "تاریخ درخواست")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(30)]
		public string? MeetingDate { get; set; }

		[Display(Name = "وضعیت قرض امتیاز")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string? PassedStatus { get; set; }

        [Display(Name = "شماره حساب امتیاز دهنده")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(13)]
        public string? ScorerAccountNumber { get; set; }

        [Display(Name = "نام امتیاز دهنده")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string? ScorerName { get; set; }

        [Display(Name = "شماره موبایل امتیاز دهنده")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11)]
        public string? ScorerMobileNumber { get; set; }

		[Display(Name = "امتیاز درخواستی")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? ReqScore { get; set; }

		[Display(Name = "وضعیت قرض امتیاز مسدودی")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(100)]
		public string? BlockPassedStatus { get; set; }

		[Display(Name = "شماره حساب امتیاز دهنده (مسدودی)")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(13)]
		public string? BlockScorerAccountNumber { get; set; }

		[Display(Name = "نام امتیاز دهنده (مسدودی)")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(150)]
		public string? BlockScorerName { get; set; }

		[Display(Name = "شماره موبایل امتیاز دهنده (مسدودی)")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(11)]
		public string? BlockScorerMobileNumber { get; set; }

		[Display(Name = "امتیاز درخواستی (مسدودی)")]
		//[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public string? BlockReqScore { get; set; }

		//[Display(Name = "تعداد اقساط تجدید نظر شده")]
		////[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		//public int RevisionCount { get; set; }

		//[Display(Name = "مبلغ تجدید نظر شده")]
		////[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		//public string RevisionAmount { get; set; }

		//[Display(Name = "پاسخ تجدید نظر خواهی")]
  //      //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
  //      public string RevisionResponse { get; set; }

  //      [Display(Name = "وضعیت")]
  //      [MaxLength(100)]
  //      public string RevisionStatus { get; set; }

        [Display(Name = "شرایط ضامنین")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string GuarantorsConditions { get; set; }

        [Display(Name = "شرایط مسدود کننده")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BlockerConditions { get; set; }

        [Display(Name = "مدت بازپرداخت (روز)")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int RepaymentPeriod { get; set; }

        [Display(Name = "نام مشتری")]
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Display(Name = "شماره حساب")]
        public int? BankAcountId { get; set; }

        [ForeignKey("BankAcountId")]
        public BankAccount? BankAccount { get; set; }  
    }
}
