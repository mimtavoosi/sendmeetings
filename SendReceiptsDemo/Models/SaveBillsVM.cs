using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class SaveBillsVM
    {
        public List<IFormFile> BillPics { get; set; }
    }
}
