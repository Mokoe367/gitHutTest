using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProject.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            CompanyContext context = HttpContext.RequestServices.GetService(typeof(CompanyProject.Models.CompanyContext)) as CompanyContext;
            return View(context.GetAllEmployees());
        }
    }
}