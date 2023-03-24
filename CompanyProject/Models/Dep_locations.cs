using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Dep_locations
    {
        [Required (ErrorMessage = "Department ID required")]
        public int depID { get; set; }

        public int pastDepID { get; set; }
        [Required(ErrorMessage = "Location name required")]
        public string loc_name { get; set; }
        public string pastLoc_name { get; set; }
    }
}
