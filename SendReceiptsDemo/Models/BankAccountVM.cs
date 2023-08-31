using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendReceiptsDemo.Models
{
    public class BankAccountVM
    {
        [Required]
        public int? BankAcountId { get; set; }
        public string AccountText { get; set; }
    }
}
