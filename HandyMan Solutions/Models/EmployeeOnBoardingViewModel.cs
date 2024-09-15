using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HandyMan_Solutions.Models
{
    public class EmployeeOnBoardingViewModel
    {
        public EmployeeOnBoarding Employee { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string SelectedRoleId { get; set; }
    }


}