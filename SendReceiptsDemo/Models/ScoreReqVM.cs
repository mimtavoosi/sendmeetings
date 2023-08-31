using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class ScoreReqVM
    {
        public int MeetId { get; set; }
        public int ReqIndex { get; set; }
        public string ReqName { get; set; }
        public string ReqAccountNumber { get; set; }
        public string ScorerAccountNumber { get; set; }

        [Required(ErrorMessage = "لطفا امتیاز درخواستی را وارد کنید")]
        [Remote("ChangeReqScore", "Home", AdditionalFields = "MeetId")]
        public string ReqScore { get; set; }
    }
}
