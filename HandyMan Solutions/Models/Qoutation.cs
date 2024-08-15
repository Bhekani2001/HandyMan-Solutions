using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class Qoutation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int qoutationKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ServiceType { get; set; }
        public string Description { get; set; } 
        public DateTime RequestedDate { get; set; }
        public string ImageUrl { get; set; }
        public string AdditionalNotes { get; set; } 

        public string TechnicianAssigned { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Paid { get; set; }
        public string Status { get; set; }

    }
}