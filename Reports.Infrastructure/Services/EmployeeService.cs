using System;
using System.Linq;
using System.Collections.Generic;
using Reports.Core.Entities;
using Reports.Core.Tools;
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
            if (bossId != null && !_context.Employees.Any(employee => employee.Id == bossId))
            {
                throw new ReportsException($"Error: employee with id {bossId} doesn't exist");
            }

            var newEmployee = new Employee(name, bossId);
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
            return newEmployee;
        }

        public void Delete(Guid employeeId)
        {
            if (!_context.Employees.Any(employee => employee.Id == employeeId))
            {
                throw new ReportsException($"Error: employee with id {employeeId} doesn't exist");
            }

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
            if (!_context.Employees.Any(employee => employee.Id == id))
            {
                throw new ReportsException($"Error: employee with id {id} doesn't exist");
            }

            return _context.Employees.FirstOrDefault(employee => employee.Id == id);
        }

        public Employee Update(Guid employeeId, Guid newBossId)
        {
            if (!_context.Employees.Any(employee => employee.Id == employeeId))
            {
                throw new ReportsException($"Error: employee with id {employeeId} doesn't exist");
            }

            if (!_context.Employees.Any(employee => employee.Id == newBossId))
            {
                throw new ReportsException($"Error: employee with id {newBossId} doesn't exist");
            }


            Employee employee = GetById(employeeId);
            employee.UpdateBoss(newBossId);
            _context.SaveChanges();
            return employee;
        }
    }
}
