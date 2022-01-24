using System.Collections.Generic;

namespace Reports.Core.Entities
{
    public class DefaultPaginator : IPaginator
    {
        private uint _employeesPerPage;
        public DefaultPaginator(uint employeesPerPage)
        {
            _employeesPerPage = employeesPerPage;
        }
        public PaginatedEmployees Paginate(List<Employee> employees)
        {
            var paginatedEmployees = new PaginatedEmployees();
            for (int i = 0; i < employees.Count; i += (int)_employeesPerPage)
            {
                var page = new List<Employee>();
                for (int j = 0; j < _employeesPerPage && i + j < employees.Count; ++j)
                {
                    page.Add(employees[i + j]);
                }
                paginatedEmployees.AddPage(page);
            }
            return paginatedEmployees;
        }
    }
}
