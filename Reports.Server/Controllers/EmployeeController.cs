using System;
using Microsoft.AspNetCore.Mvc;
using Reports.Infrastructure.Services;
using Reports.Core.Entities;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("create")]
        public Employee Create([FromQuery] string name, [FromQuery] Guid? bossId = null)
        {
            return _employeeService.Create(name, bossId);
        }

        [HttpPost]
        [Route("delete")]
        public void Delete([FromQuery] Guid employeeId)
        {
            _employeeService.Delete(employeeId);
        }

        [HttpPost]
        [Route("update")]
        public void Update([FromQuery] Guid employeeId, [FromQuery] Guid newBossId)
        {
            _employeeService.Update(employeeId, newBossId);
        }

        [HttpGet]
        [Route("get")]
        public Employee GetById([FromQuery] Guid id)
        {
            return _employeeService.GetById(id);
        }

        [HttpGet]
        [Route("getAll")]
        public PaginatedEmployees GetAll([FromQuery] EmployeeFilter employeeFilter, [FromQuery] IPaginator paginator)
        {
            return _employeeService.GetAll(employeeFilter, paginator);
        }
    }
}