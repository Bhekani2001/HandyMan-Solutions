using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class SelectedEmployeeName
    {
        public int EmployeeKey { get; set; }
        public string FullName { get; set; } // This combines FirstName, LastName, and FamilyName
        public string RoleName { get; set; } // Role of the employee
    }
}