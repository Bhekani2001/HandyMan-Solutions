using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class TaskProcessingViewModel
    {
        public int ServiceProvidedId { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserContact { get; set; }
        public string UserEmail { get; set; }
        public string ServiceType { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerEmail { get; set; }
        public string QoutationNotes { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

}