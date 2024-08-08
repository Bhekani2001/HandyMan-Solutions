using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class EmployeeOnBoarding
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeKey { get; set; }
        public string EFirstName { get; set; }
        public string ELastName { get; set; }
        public string EFamilyName { get; set; }
        public string EIdentityNumber { get; set; }
        public DateTime EDateofBirth { get; set; }
        public string EContact { get; set; }
        public string EEmailAddress { get; set; }
        public int EYearsofExperience { get; set; }
        public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}
