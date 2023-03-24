using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Department
    {
        private CompanyContext context;
        public int depID { get; set; }
        [Required]
        public string location { get; set; }
        [Required]
        public string depName { get; set; }
        public int mgrID { get; set; }    
        public int deleted_flag { get; set; }
    }
}
