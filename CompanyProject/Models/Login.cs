using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class Login
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string privilege { get; set; }
    }
}
