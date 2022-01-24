using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Core.Entities
{
    public class EmployeeFilter
    {
        private IEmployeeChecker _employeeChecker;
        public EmployeeFilter(IEmployeeChecker employeeChecker)
        {
            _employeeChecker = employeeChecker;
        }

        public List<Employee> Filtrate(List<Employee> employees)
        {
            var filtratedEmployees = new List<Employee>();
            foreach (Employee employee in employees)
            {
                if (_employeeChecker.Check(employee))
                {
                    filtratedEmployees.Add(employee);
                }
            }

            return filtratedEmployees;
        }
    }
}
