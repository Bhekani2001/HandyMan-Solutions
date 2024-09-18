using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class ChartViewModel
    {
        public string[] Dates { get; set; }
        public int[] PaidCounts { get; set; }
        public int[] NewPaidCounts { get; set; }
    }

}