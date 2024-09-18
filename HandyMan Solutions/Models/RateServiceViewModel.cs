using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class RateServiceViewModel
    {
        public string ServiceId { get; set; }
        public string ServiceType { get; set; }
        public decimal Rating { get; set; }
        public string Comments { get; set; }
    }
}