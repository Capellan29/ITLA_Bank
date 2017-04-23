using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Highlighted { get; set; }
        public string  NotificationType { get; set; }
    }
}