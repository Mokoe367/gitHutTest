using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyProject.Models
{
    public class CompanyContext
    {
        public string ConnectionString { get; set; }

        public CompanyContext(string connectionString)
        {
            this.ConnectionString = connectionString;
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

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> list = new List<Employee>();

            using(MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from employee;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       
                        DateTime date = Convert.ToDateTime(getStringValue(reader["birthdate"]));
                        string dateNoTime = date.ToShortDateString();
                        list.Add(new Employee()
                        {
                            ID = getIntValue(reader["employeeID"]),
                            Fname = getStringValue(reader["Fname"]),
                            Lname = getStringValue(reader["Lname"]),
                            Mname = getStringValue(reader["Mname"]),
                            Address = getStringValue(reader["address"]),
                            Sex = getStringValue(reader["sex"]),
                            BirthDate = dateNoTime,
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
            }

            return list;
        }
    }
}
