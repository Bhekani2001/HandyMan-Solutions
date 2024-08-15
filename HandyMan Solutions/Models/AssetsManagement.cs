using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandyMan_Solutions.Models
{
    public class AssetsManagement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  assetskey{ get; set; }

        public string  Assets { get; set; }
    }

    public class CarOnboarding
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int caronboardingkey { get; set; }

        public string  CarName { get; set; }

        public string  CarType { get; set; }

        public string  CarModel { get; set; }
        public string  CarMake { get; set; }
        [RegularExpression(@"^\d{3}-\d{3}$", ErrorMessage = "Invalid registration number format. Use 'NNN-NNN' (e.g., 233-657).")]
        public string CarRegistrationNumber { get; set; }
        public DateTime AcquisitionDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DiscExpiryDate { get; set; }
        public string  CarStatus { get; set; }
    }
}