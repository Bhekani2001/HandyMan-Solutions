using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class WeeklyUserRegistrationsViewModel
    {
        public List<string> Weeks { get; set; }
        public List<int> UserCounts { get; set; }
    }
}