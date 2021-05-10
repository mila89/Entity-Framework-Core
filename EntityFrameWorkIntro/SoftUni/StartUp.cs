using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dbContext = new SoftUniContext();
            string res = GetEmployeesInPeriod(dbContext);
            Console.WriteLine(res);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            //  Find the first 10 employees who have projects started in the period 2001 - 2003(inclusive).
            //Print each employee's first name, last name, manager’s first name and last name. Then return all 
            //of their projects in the format "--<ProjectName> - <StartDate> - <EndDate>",
            //each on a new row. If a project has no end date, print "not finished" instead

            var employees = context.Employees
                            .Include(x => x.EmployeesProjects)
                            .ThenInclude(x => x.Project)
                            .Where(p => p.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                            .Take(10).ToList();
            StringBuilder sb = new StringBuilder();
            
            foreach (var empl in employees)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} - Manager: {empl.Manager.FirstName} {empl.Manager.LastName}");
                foreach (var project in empl.EmployeesProjects)
                {
                    if (project.Project.EndDate.HasValue)
                    {
                        sb.AppendLine($"--{project.Project.Name} - {project.Project.StartDate} - {project.Project.EndDate}");
                    }
                    else
                    {
                        sb.AppendLine($"--{project.Project.Name} - {project.Project.StartDate} - not finished");
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        var address = new Address();
        {
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;
        };
        context.Addresses.Add(address);
        Employee employee = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
        employee.Address = address;
        context.SaveChanges();
        var emplList = context.Employees.OrderByDescending(x => x.AddressId).Select
            (x => new
            {
                x.Address.AddressText
            }).Take(10).ToList();
        StringBuilder sb = new StringBuilder();
        foreach (var empl in emplList)
        {
            sb.AppendLine($"{empl.AddressText}");
        }
        return sb.ToString().TrimEnd();
    }

}
}
