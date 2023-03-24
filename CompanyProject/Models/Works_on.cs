using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Works_on
    {
        [Required (ErrorMessage = "EmployeeID Required")]
        public int employeeID { get; set; }
        [Required(ErrorMessage = "TaskID Required")]
        public int TaskID { get; set; }
        public decimal hours { get; set; }
        public int deleted_flag { get; set; }
        public int tempemployeeID { get; set; }
        public int tempTaskID { get; set; }
       
    }
}
