using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CompanyProject.Controllers
{
    public class ManagerController : Controller
    {
        public int userDepID { set; get; }
        public int userEmpID { set; get; }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection("server = localhost; port=3306;database=target;user=root;password=MonkeysInc7!");
        }

        public static string getStringValue(object value)
        {
            if (value == DBNull.Value) return string.Empty;
            return value.ToString();
        }

        public int getIntValue(object value)
        {
            if (value == DBNull.Value) return 0;
            return Convert.ToInt32(value);
        }
       
        public IActionResult Index()
        {
            string empID = HttpContext.Session.GetString("id");

            if (empID == null)
            {
                return RedirectToAction("Login", "Login", new { error = "Session Timed out" });
            }
            else
            {               
                MySqlConnection conn = GetConnection();
                conn.Open();
                Employee user = new Employee();
                MySqlCommand cmd = new MySqlCommand("select e.Fname, e.Lname, e.depID from employee as e where e.employeeID = '" + empID + "'", conn);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user.Fname = getStringValue(reader["Fname"]);
                    user.Lname = getStringValue(reader["Lname"]);
                    user.DepID = getIntValue(reader["depID"]);
                }
                conn.Close();
                HttpContext.Session.SetString("depID", user.DepID.ToString());               
                userDepID = user.DepID;
                userEmpID = Convert.ToInt32(empID);
                string msg = "Signed in as " + user.Fname + " " + user.Lname + " showing Department " + user.DepID;
                ViewData["userInfo"] = msg;               
                return View(getViewData());
            }
                         
        }

        public IActionResult AddEmployee()
        {
            string depID = "Adding Employee into Department number " + HttpContext.Session.GetString("depID");
            ViewData["AddInfo"] = depID;
            return View();
        }

        public IActionResult EditEmployee(int id)
        {
            var emp = new Employee();

            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from employee where employeeID = " + id + "; ", conn);

            var reader = cmd.ExecuteReader();
            reader.Read();
            DateTime date = Convert.ToDateTime(getStringValue(reader["birthdate"]));
            string dateNoTime = date.ToShortDateString();
            string[] dateTemp = dateNoTime.Split('/');
            int month = Int32.Parse(dateTemp[0]);
            int day = Int32.Parse(dateTemp[1]);
            if (month < 10)
            {
                dateTemp[0] = "0" + dateTemp[0];
            }
            if (day < 10)
            {
                dateTemp[1] = "0" + dateTemp[1];
            }
            string sqlDate = dateTemp[2] + "-" + dateTemp[0] + "-" + dateTemp[1];

            emp.Fname = getStringValue(reader["Fname"]);
            emp.ID = getIntValue(reader["employeeID"]);
            emp.Fname = getStringValue(reader["Fname"]);
            emp.Lname = getStringValue(reader["Lname"]);
            emp.Mname = getStringValue(reader["Mname"]);
            emp.Address = getStringValue(reader["address"]);
            emp.Sex = getStringValue(reader["sex"]);
            emp.BirthDate = sqlDate;
            emp.RoleID = getIntValue(reader["roleId"]);
            emp.DepID = getIntValue(reader["depId"]);
            emp.Ssn = getIntValue(reader["ssn"]);
            emp.Salary = getIntValue(reader["salary"]);
            emp.SuperID = getIntValue(reader["superID"]);


            return View(emp);         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(Employee obj)
        {
            
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select employee.ssn from employee where employee.ssn = " + obj.Ssn + "; ", conn);
            string empID = HttpContext.Session.GetString("id");
            obj.ID = Convert.ToInt32(empID);
            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                ModelState.AddModelError("Ssn", "No Duplicate SSN");
            }
            reader.Close();
            string query = "select roleId from roles where roleId = " + obj.RoleID + ";";
            cmd.CommandText = query;
            cmd.Connection = conn;
            reader = cmd.ExecuteReader();
            if (!reader.HasRows && obj.RoleID != 0)
            {
                ModelState.AddModelError("RoleId", "Role number doesn't exist");
            }
            reader.Close();
            query = "select employeeID from employee where employeeID = " + obj.SuperID + ";";
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();
            if (!reader.HasRows && obj.SuperID != 0)
            {
                ModelState.AddModelError("superID", "superviser Id number doesn't exist");
            }
            if (reader.HasRows && obj.SuperID != 0)
            {
                reader.Read();
                int id = getIntValue(reader["employeeID"]);
                if (id == obj.ID)
                {
                    ModelState.AddModelError("superID", "supervisor can't be same as employeeID");
                }

            }
            reader.Close();
            if (obj.Sex != "M" && obj.Sex != "F")
            {
                ModelState.AddModelError("Sex", "Gender Must be either M or F");
            }
            if (ModelState.IsValid)
            {
                string depID = HttpContext.Session.GetString("depID");
                int department = Convert.ToInt32(depID);
                MySqlCommand insert = new MySqlCommand();
                query = "insert into employee(Fname, Mname, Lname, salary, ssn, address, birthDate, sex, roleID, superID, depID) " +
                    "Values( @Fname, @Mname, @Lname, @salary , @ssn , @address, @birthdate, @sex " +
                    ", @roleId, @superID, @depId);";
                insert.CommandText = query;
                insert.Parameters.AddWithValue("@Fname", obj.Fname);
                insert.Parameters.AddWithValue("@Mname", obj.Mname);
                insert.Parameters.AddWithValue("@Lname", obj.Lname);
                insert.Parameters.AddWithValue("@sex", obj.Sex);
                insert.Parameters.AddWithValue("@birthdate", obj.BirthDate);
                insert.Parameters.AddWithValue("@salary", obj.Salary);
                insert.Parameters.AddWithValue("@ssn", obj.Ssn);
                insert.Parameters.AddWithValue("@address", obj.Address);
                insert.Parameters.AddWithValue("@depId", department);
                if (obj.RoleID == 0)
                {
                    insert.Parameters.AddWithValue("@roleId", DBNull.Value);
                }
                else
                {
                    insert.Parameters.AddWithValue("@roleId", obj.RoleID);
                }

                if (obj.SuperID == 0)
                {
                    insert.Parameters.AddWithValue("@superID", DBNull.Value);
                }
                else
                {
                    insert.Parameters.AddWithValue("@superID", obj.SuperID);
                }
                insert.Connection = conn;
                insert.ExecuteNonQuery();
                conn.Close();
                TempData["success"] = "Employee added successfully";
                return RedirectToAction("Index");


            }
            else
            {
                string depID = "Adding Employee into Department number " + HttpContext.Session.GetString("depID");
                ViewData["AddInfo"] = depID;
                return View(obj);
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEmployee(Employee employee)
        {

            MySqlConnection conn = GetConnection();
            conn.Open();
                        
            MySqlCommand cmd2 = new MySqlCommand("select depID from department where depID = " + employee.DepID + ";", conn);
            var reader = cmd2.ExecuteReader();
            if (!reader.Read() && employee.DepID != 0)
            {
                ModelState.AddModelError("DepId", "Department number doesn't exist");
            }
            reader.Close();

            cmd2 = new MySqlCommand("select roleId from roles where roleId = " + employee.RoleID + ";", conn);
            reader = cmd2.ExecuteReader();
            if (!reader.Read() && employee.RoleID != 0)
            {
                ModelState.AddModelError("RoleId", "Role number doesn't exist");
            }
            reader.Close();

            cmd2 = new MySqlCommand("select employeeID from employee where employeeID = " + employee.SuperID + ";", conn);
            reader = cmd2.ExecuteReader();
            if (!reader.HasRows && employee.SuperID != 0)
            {
                ModelState.AddModelError("superID", "superviser Id number doesn't exist");
            }
            if (reader.Read() && employee.SuperID != 0)
            {
                int id = getIntValue(reader["employeeID"]);
                if (id == employee.ID)
                {
                    ModelState.AddModelError("superID", "supervisor can't be same as employeeID");
                }

            }
            reader.Close();

            if (employee.Sex != "M" && employee.Sex != "F")
            {
                ModelState.AddModelError("Sex", "Gender Must be either M or F");
            }

            if (ModelState.IsValid)
            {

                string query = "UPDATE employee SET salary=@salary," +
                    " roleId=@roleId, superID=@superID where employeeID = " + employee.ID + ";";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;               
                cmd.Parameters.AddWithValue("@salary", employee.Salary);                              
                if (employee.RoleID == 0)
                {
                    cmd.Parameters.AddWithValue("@roleId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@roleId", employee.RoleID);
                }

                if (employee.SuperID == 0)
                {
                    cmd.Parameters.AddWithValue("@superID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@superID", employee.SuperID);
                }

                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                TempData["success"] = "Employee edited successfully";
                return RedirectToAction("Index");

            }


            return View(employee);
        }

        public IActionResult FireEmployee(int id)
        {
            var emp = new Employee();

            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from employee where employeeID = " + id + "; ", conn);

            var reader = cmd.ExecuteReader();
            reader.Read();
            DateTime date = Convert.ToDateTime(getStringValue(reader["birthdate"]));
            string dateNoTime = date.ToShortDateString();
            string[] dateTemp = dateNoTime.Split('/');
            int month = Int32.Parse(dateTemp[0]);
            int day = Int32.Parse(dateTemp[1]);
            if (month < 10)
            {
                dateTemp[0] = "0" + dateTemp[0];
            }
            if (day < 10)
            {
                dateTemp[1] = "0" + dateTemp[1];
            }
            string sqlDate = dateTemp[2] + "-" + dateTemp[0] + "-" + dateTemp[1];

            emp.Fname = getStringValue(reader["Fname"]);
            emp.ID = getIntValue(reader["employeeID"]);
            emp.Fname = getStringValue(reader["Fname"]);
            emp.Lname = getStringValue(reader["Lname"]);
            emp.Mname = getStringValue(reader["Mname"]);
            emp.Address = getStringValue(reader["address"]);
            emp.Sex = getStringValue(reader["sex"]);
            emp.BirthDate = sqlDate;
            emp.RoleID = getIntValue(reader["roleId"]);
            emp.DepID = getIntValue(reader["depId"]);
            emp.Ssn = getIntValue(reader["ssn"]);
            emp.Salary = getIntValue(reader["salary"]);
            emp.SuperID = getIntValue(reader["superID"]);


            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FireEmployee(Employee employee)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("select employee.deleted_flag from employee where employee.employeeID = " + employee.ID + ";", conn);
            var reader = cmd.ExecuteReader();
            reader.Read();
            int flag = getIntValue(reader["deleted_flag"]);
            reader.Close();

            string query = "UPDATE employee SET deleted_flag=@deleted_flag where employeeID = " + employee.ID + ";";
            cmd.CommandText = query;
            if (flag == 1)
            {
                cmd.Parameters.AddWithValue("@deleted_flag", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@deleted_flag", 1);
            }
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        public IActionResult TaskDetails(int id)
        {
            IEnumerable<TaskDetails> list = getTaskDetails(id);                    

            
            MySqlConnection conn = GetConnection();
                conn.Open();
                Employee user = new Employee();
                MySqlCommand cmd = new MySqlCommand("select e.Fname, e.Lname from employee as e where e.employeeID = '" + id + "'", conn);
                var reader = cmd.ExecuteReader();               
                reader.Read();
                user.Fname = getStringValue(reader["Fname"]);
                user.Lname = getStringValue(reader["Lname"]);

                conn.Close();

                string msg = "Task Data for " + user.Fname + " " + user.Lname;
                ViewData["TaskInfo"] = msg;

                ViewData["employee"] = id;

                return View(list);
           
           
        }

        public IActionResult Unlist(int empid, int taskid)
        {
            Works_on work = new Works_on();

            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select deleted_flag from works_on where employeeID = " + empid + " and " +
                "taskID = " + taskid + ";", conn);
            var reader = cmd.ExecuteReader();
            reader.Read();
            int flag = getIntValue(reader["deleted_flag"]);
            reader.Close();

            string query = "UPDATE works_on SET deleted_flag=@deleted_flag where employeeID = " + empid + " and " +
                "taskID = " + taskid + ";";
            cmd.CommandText = query;
            if (flag == 1)
            {
                cmd.Parameters.AddWithValue("@deleted_flag", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@deleted_flag", 1);
            }
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            return RedirectToAction("TaskDetails", new { id=empid });
        }

        public IActionResult Enlist(int id)
        {
            Works_on work = new Works_on();
            work.employeeID = id;
            return View(work);
        }

        public IEnumerable<ManagerViewModel> getViewData()
        {
            List<ManagerViewModel> data = new List<ManagerViewModel>();
            ManagerViewModel model = new ManagerViewModel();

            model.Employees = getEmployeeData();            
            model.Projects = getProjectData();           

            data.Add(model);

            return data;

        }

        public List<Employee> getEmployeeData()
        {
            MySqlConnection conn = GetConnection();
            List<Employee> employeeData = new List<Employee>();

            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from employee where employee.depID = " + userDepID + " and employee.deleted_flag != 0;", conn);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    DateTime date = Convert.ToDateTime(getStringValue(reader["birthdate"]));
                    string dateNoTime = date.ToShortDateString();
                    string[] dateTemp = dateNoTime.Split('/');
                    int month = Int32.Parse(dateTemp[0]);
                    int day = Int32.Parse(dateTemp[1]);
                    if (month < 10)
                    {
                        dateTemp[0] = "0" + dateTemp[0];
                    }
                    if (day < 10)
                    {
                        dateTemp[1] = "0" + dateTemp[1];
                    }
                    string sqlDate = dateTemp[2] + "-" + dateTemp[0] + "-" + dateTemp[1];
                    employeeData.Add(new Employee()
                    {
                        ID = getIntValue(reader["employeeID"]),
                        Fname = getStringValue(reader["Fname"]),
                        Lname = getStringValue(reader["Lname"]),
                        Mname = getStringValue(reader["Mname"]),
                        Address = getStringValue(reader["address"]),
                        Sex = getStringValue(reader["sex"]),
                        BirthDate = sqlDate,
                        Deleted_flag = getIntValue(reader["deleted_flag"]),
                        RoleID = getIntValue(reader["roleId"]),
                        DepID = getIntValue(reader["depId"]),
                        Ssn = getIntValue(reader["ssn"]),
                        Salary = getIntValue(reader["salary"]),
                        SuperID = getIntValue(reader["superID"])

                    });
                }
            }
            conn.Close();
            return employeeData;
        }

        public List<Project> getProjectData()
        {
            MySqlConnection conn = GetConnection();
            List<Project> projectData = new List<Project>();

            conn.Open();

            MySqlCommand cmd = new MySqlCommand("select * from project where project.depID = " + userDepID + " and project.deleted_flag != 0;", conn);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime date = Convert.ToDateTime(getStringValue(reader["dueDate"]));
                    string dateNoTime = date.ToShortDateString();
                    string[] dateTemp = dateNoTime.Split('/');
                    int month = Int32.Parse(dateTemp[0]);
                    int day = Int32.Parse(dateTemp[1]);
                    if (month < 10)
                    {
                        dateTemp[0] = "0" + dateTemp[0];
                    }
                    if (day < 10)
                    {
                        dateTemp[1] = "0" + dateTemp[1];
                    }
                    string sqlDate = dateTemp[2] + "-" + dateTemp[0] + "-" + dateTemp[1];
                    projectData.Add(new Project()
                    {
                        projID = getIntValue(reader["projID"]),
                        dueDate = sqlDate,
                        depID = getIntValue(reader["depID"]),
                        projName = getStringValue(reader["projName"]),
                        location = getStringValue(reader["location"]),
                        cost = getIntValue(reader["cost"]),
                        projStatus = Convert.ToDecimal(reader["projStatus"]),
                        field = getStringValue(reader["field"]),
                        deleted_flag = getIntValue(reader["deleted_flag"])
                    });
                }
            }
            conn.Close();

            return projectData;
        }

        public List<TaskDetails> getTaskDetails(int id)
        {
            List<TaskDetails> TaskData = new List<TaskDetails>();
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select w.employeeID, w.hours, w.taskID, t.taskName, t.cost, t.taskDueDate, t.projID " +
                "from works_on as w right outer join task as t on t.taskID = w.taskID where w.employeeID = " + id + " and w.deleted_flag = 1;", conn);

            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    DateTime date = Convert.ToDateTime(getStringValue(reader["taskDueDate"]));
                    string dateNoTime = date.ToShortDateString();
                    string[] dateTemp = dateNoTime.Split('/');
                    int month = Int32.Parse(dateTemp[0]);
                    int day = Int32.Parse(dateTemp[1]);
                    if (month < 10)
                    {
                        dateTemp[0] = "0" + dateTemp[0];
                    }
                    if (day < 10)
                    {
                        dateTemp[1] = "0" + dateTemp[1];
                    }
                    string sqlDate = dateTemp[2] + "-" + dateTemp[0] + "-" + dateTemp[1];

                    TaskData.Add(new TaskDetails()
                    {
                        empID = getIntValue(reader["employeeID"]),
                        taskID = getIntValue(reader["taskID"]),
                        projID = getIntValue(reader["projID"]),
                        dueDate = sqlDate,
                        hours = getIntValue(reader["hours"]),
                        taskName = getStringValue(reader["taskName"]),
                        budget = getIntValue(reader["cost"])
                    });
                    
                }
            }
            conn.Close();

            return TaskData;
        }
    }
}