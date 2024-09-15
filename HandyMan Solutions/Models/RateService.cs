using System;
using System.Collections.Generic;

namespace HandyMan_Solutions.Models
{
    public class RateService
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public decimal Rating { get; set; } 
        public string Comments { get; set; }

        public List<ServiceProvided> UserServices { get; set; }

        public RateService()
        {
            UserServices = new List<ServiceProvided>();
        }
    }
}
