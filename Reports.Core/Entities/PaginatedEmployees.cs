using System.Collections.Generic;

namespace Reports.Core.Entities
{
    public class PaginatedEmployees
    {
        private List<List<Employee>> _pages; 

        public PaginatedEmployees()
        {
            _pages = new List<List<Employee>>();
        }

        public IReadOnlyList<Employee> GetPage(uint index)
        {
            return _pages[(int)index];
        }

        internal void AddPage(List<Employee> page)
        {
            _pages.Add(page);
        }
    }
}
