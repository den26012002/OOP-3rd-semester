using System.Collections.Generic;

namespace Reports.Core.Entities
{
    public interface IPaginator
    {
        PaginatedEmployees Paginate(List<Employee> employees);
    }
}
