using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace CompanyProject.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login(string error = "")
        {
            if(error != "")
            {
                ViewData["LoginFlag"] = error;
            }            
            return View();
        }

        public string HashPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            var passwordBytes = Encoding.Default.GetBytes(password);
            var hashedpassword = hash.ComputeHash(passwordBytes);

            return BitConverter.ToString(hashedpassword).Replace("-","");
        }

        [HttpPost]
        public ActionResult Verify(Login acc)
        {
            MySqlConnection conn = new MySqlConnection("server = localhost; port=3306;database=target;user=root;password=MonkeysInc7!");
            
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from login where username='"+acc.username+"';", conn);
            var reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                string salt = reader["salt"].ToString();
                if (HashPassword($"{acc.password}{salt}") == reader["hash"].ToString()) 
                {
                    string privilege = reader["user_privilege"].ToString();
                    int empID = Convert.ToInt32(reader["employeeID"]);
                    reader.Close();

                    MySqlCommand cmd2 = new MySqlCommand("select e.deleted_flag from employee as e, login as l where l.employeeID = e.employeeID and e.employeeID ='" + empID + "'", conn);
                    var reader2 = cmd2.ExecuteReader();
                    int flag = -1;
                    if (reader2.Read())
                    {
                        flag = Convert.ToInt16(reader2["deleted_flag"]);
                    }
                    reader2.Close();

                    if (privilege == "Admin" && flag == 1)
                    {
                        conn.Close();
                        HttpContext.Session.SetString("id", empID.ToString());
                        return RedirectToAction("Index", "AdminView");
                    }
                    else if (privilege == "Employee" && flag == 1)
                    {
                        conn.Close();
                        TempData["id"] = empID;
                        return RedirectToAction("Index", "Employee");
                    }
                    else if (privilege == "Manager" && flag == 1)
                    {
                        conn.Close();
                        HttpContext.Session.SetString("id", empID.ToString());
                        return RedirectToAction("Index", "Manager");
                    }
                    else if (flag == 0)
                    {
                        ViewData["LoginFlag"] = "Deleted User";
                        return View("Login");
                    }

                    ViewData["LoginFlag"] = "Something went Wrong";
                    return View("Login");
                }
                else
                {
                    conn.Close();
                    ViewData["LoginFlag"] = "Wrong Username or Password";
                    return View("Login");
                }
            }
            else
            {
                conn.Close();
                ViewData["LoginFlag"] = "Unregistered User";
                return View("Login");
            }                     
                             
        }
        
    }
}