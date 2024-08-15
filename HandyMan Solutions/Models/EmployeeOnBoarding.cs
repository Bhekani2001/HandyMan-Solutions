using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string EContact { get; set; }
        public string ESecondContact { get; set; }
        public string EEmailAddress { get; set; }
        public string EAddress { get; set; }
        public string EStatus{ get; set; }
        public int EYearsofExperience { get; set; }

        [Required]
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
