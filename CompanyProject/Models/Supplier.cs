using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Supplier
    {
        public int supID { get; set; }
        [Required(ErrorMessage = "Product Required")]
        public string product { get; set; }
        [Required (ErrorMessage = "Name Required")]
        public string name { get; set; }
        public int roleID { get; set; }
        public int deleted_flag { get; set; }

    }
}
