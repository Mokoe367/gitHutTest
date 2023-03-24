using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Tasks
    {
        public int taskID { get; set; }
        [Required (ErrorMessage = "Task Name Required")]
        public string taskName { get; set; }
        [Required(ErrorMessage = "Budget Required")]
        public int cost { get; set; }
        [Required(ErrorMessage = "Due Date Required")]
        [RegularExpression("([12]\\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01]))", ErrorMessage = "Invalid Date Format")]
        public string taskDueDate { get; set; }
        public int projID { get; set; }       
        public int deleted_flag { get; set; }
    }
}
