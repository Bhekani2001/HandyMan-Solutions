using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class RequestQoutation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int qoutationKey { get; set; }
        public string UserId { get; set; }
        public string ServiceType { get; set; } // Type of service requested (e.g., Appliance Repair, HVAC, Carpentry, Electrical, Plumbing, General Maintenance)
        public string Description { get; set; }
        public DateTime RequestedDate { get; set; }
        public List<string> Images { get; set; }
        public string AdditionalNotes { get; set; }

        // HandyMan-related properties
        public string TechnicianAssigned { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Paid { get; set; }
        public string Status { get; set; }

    }
}