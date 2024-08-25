using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class ServiceProvided
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserContact { get; set; }
        public string UserEmail { get; set; }
        public string ServiceType { get; set; }
        public string Paid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }


        public string TechnicalNotes { get; set; }
        public string TechnicalStatus { get; set; }
        public string TechnicianAssigned { get; set; }

    }
}