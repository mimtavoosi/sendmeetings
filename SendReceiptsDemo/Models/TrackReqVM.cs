using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SendReceiptsDemo.Models
{
    public class TrackReqVM
    {
        public string Status { get; set; }
        public string Response { get; set; }
    }
}
