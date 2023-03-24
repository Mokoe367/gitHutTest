using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Used_by
    {
        [Required (ErrorMessage = "EmployeeID Required")]
        public int employeeID { get; set; }
        [Required(ErrorMessage = "SupplierID Required")]
        public int supID { get; set; }
        [Required(ErrorMessage = "AssetID Required")]
        public int assetID { get; set; }
        public int deleted_flag { get; set; }
        public decimal status { get; set; }
        public int tempemployeeID { get; set; }
        public int tempsupID { get; set; }
        public int tempassetID { get; set; }
    }
}
