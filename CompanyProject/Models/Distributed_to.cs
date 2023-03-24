using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Distributed_to
    {   
        [Required (ErrorMessage = "Supplier ID required")]
        public int supID { get; set; }
        [Required(ErrorMessage = "Asset ID required")]
        public int assetID { get; set; }
        [Required(ErrorMessage = "Department ID required")]
        public int depID { get; set; }
        public int deleted_flag { get; set; }
        public int tempSupID { get; set; }
        public int tempAssetID { get; set; }
        public int tempDepID { get; set; }
        public decimal status { get; set; }
    }
}
