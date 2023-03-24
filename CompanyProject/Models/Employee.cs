using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Employee
    {
        private CompanyContext context;
        
        public int ID { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        public string Lname { get; set; }

        public string Mname { get; set; }

        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Birth Day Required")]
        [RegularExpression("([12]\\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01]))", ErrorMessage = "Invalid Date Format")]
        public string BirthDate{ get; set; }

        public int Deleted_flag { get; set; }

        public int RoleID { get; set; }

        public int DepID { get; set; }

        [Required(ErrorMessage = "SSN Required")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "Invalid SSN Format")]
        public int Ssn { get; set; }

        [Required(ErrorMessage = "Salary Required")]
        [Range(0,100000,ErrorMessage = "Range should be between 0 and 100k")]
        public int Salary { get; set; }

        public int SuperID { get; set; }


    }
}
