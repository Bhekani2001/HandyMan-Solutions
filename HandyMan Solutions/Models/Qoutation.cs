using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HandyMan_Solutions.Models
{
    public class Qoutation
    {
        public int Id { get; set; }

        public string ServiceType { get; set; }

        public string Description { get; set; }

        public string AdditionalNotes { get; set; }
        public string Status { get; set; }
        public string UrgencyLevel { get; set; }
        public string PropertyType { get; set; }

        public byte[] ImageData { get; set; }

        public DateTime RequestedDate { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
        public string TechnicianAssigned { get; set; }
        public string TechnicianNotes { get; set; }
        public string Paid { get; set; }
        public string NewPaid { get; set; }
        public decimal EstimatedCost { get; set; }

        public EmployeeOnBoarding Employees { get; set; }
    }
}
