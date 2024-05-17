using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.Models
{
    public class NotificationItem
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
    }
}