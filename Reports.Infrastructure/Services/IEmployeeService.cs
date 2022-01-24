using System;
using System.Collections.Generic;
using Reports.Core.Entities;

namespace Reports.Infrastructure.Services
{
    public interface IEmployeeService
    {
        PaginatedEmployees GetAll(EmployeeFilter employeeFilter, IPaginator paginator);
        Employee GetById(Guid id);
        Employee Update(Guid employeeId, Guid newBossId);
        void Delete(Guid employeeId);
        Employee Create(string name, Guid? bossId = null);
    }
}
