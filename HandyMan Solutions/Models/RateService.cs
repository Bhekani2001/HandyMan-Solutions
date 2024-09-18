using System;
using System.Collections.Generic;

namespace HandyMan_Solutions.Models
{
    public class RateService
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ServiceId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal Rating { get; set; }
        public string Comments { get; set; }
        public bool Rated { get; set; } 

        public ServiceProvided ServiceProvided { get; set; }
    }
}
