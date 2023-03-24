using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class AdminViewModel
    {
        public List<Employee> Employees { get; set; }
        public List<Department> Departments { get; set; }
        public List<Project> Projects { get; set; }
        public List<Role> Roles { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Tasks> Tasks { get; set; }
        public List<Asset> Assets { get; set; }
        public List<Dep_locations> Locations {get; set;}
        public List<Distributed_to> Distributions { get; set; }
        public List<Used_by> UsedBy { get; set; }
        public List<Works_on> Works_Ons { get; set; }

    }
}
