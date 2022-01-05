using System;
using System.Linq;
using System.Collections.Generic;
using Reports.Core.Entities;
using Reports.Infrastructure.DAL;

namespace Reports.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private ReportsContext _context;
        public EmployeeService(ReportsContext context)
        {
            _context = context;
        }
        public Employee Create(string name, Guid? bossId = null)
        {
            var newEmployee = new Employee(name, bossId);
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
            return newEmployee;
        }

        public void Delete(Guid employeeId)
        {
            Employee employee = GetById(employeeId);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public PaginatedEmployees GetAll(EmployeeFilter employeeFilter, IPaginator paginator)
        {
            return paginator.Paginate(employeeFilter.Filtrate(_context.Employees.ToList()));
        }

        public Employee GetById(Guid id)
        {
            return _context.Employees.FirstOrDefault(employee => employee.Id == id);
        }

        public Employee Update(Guid employeeId, Guid newBossId)
        {
            Employee employee = GetById(employeeId);
            employee.UpdateBoss(newBossId);
            _context.SaveChanges();
            return employee;
        }
    }
}
