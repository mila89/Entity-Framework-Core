using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dbContext = new SoftUniContext();
            //string res = GetEmployeesFullInformation(dbContext);
            string res = GetEmployeesWithSalaryOver50000(dbContext);
            Console.WriteLine(res);
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Select(x => new
            {
                x.FirstName,
                x.Salary
            });
            StringBuilder sb = new StringBuilder();
            foreach (var empl in employees.Where(x=>x.Salary>50000).OrderBy(y=>y.FirstName).ToList())
            {
                sb.AppendLine($"{empl.FirstName} - {empl.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employee = context.Employees.Select(
                x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary
                });
            StringBuilder sb = new StringBuilder();
            foreach (var empl in employee.OrderBy(x => x.EmployeeId).ToList())
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} {empl.MiddleName} {empl.JobTitle} {empl.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
