using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredOn { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}
