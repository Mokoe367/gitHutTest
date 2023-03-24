using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Role
    {
        private CompanyContext context;
        public int roleID { get; set; }
        [Required(ErrorMessage = "Role name required")]
        public string roleName { get; set; }

    }
}
