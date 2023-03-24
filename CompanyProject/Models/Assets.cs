using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Asset
    {
        public int assetID { get; set; }
        [Required(ErrorMessage = "Type Required")]
        public string type { get; set; }
        [Required(ErrorMessage = "Cost Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Range should be more than 0")]
        public int cost { get; set; }
        public int supID { get; set; }
        public int deleted_flag { get; set; }
    }
}
