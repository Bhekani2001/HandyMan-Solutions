using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class SelectQuotationAndEmployeeViewModel
    {
        public List<Qoutation> Quotations { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
    }

    public class EmployeeViewModel
    {
        public int EmployeeKey { get; set; }
        public string EFirstName { get; set; }
        public string ELastName { get; set; }
        public string EFamilyName { get; set; }
        public string RoleName { get; set; }

        public string FullNameWithRole
        {
            get
            {
                return $"{EFirstName} {ELastName} {EFamilyName} - {RoleName}";
            }
        }
    }


}