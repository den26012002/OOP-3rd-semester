using System;
using Microsoft.AspNetCore.Mvc;
using Reports.Infrastructure.Services;
using Reports.Core.Entities;
using Reports.Core.Tools;

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
        public IActionResult Create([FromQuery] string name, [FromQuery] Guid? bossId = null)
        {
            try
            {
                return Ok(_employeeService.Create(name, bossId));
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete([FromQuery] Guid employeeId)
        {
            try
            {
                _employeeService.Delete(employeeId);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromQuery] Guid employeeId, [FromQuery] Guid newBossId)
        {
            try
            {
                _employeeService.Update(employeeId, newBossId);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetById([FromQuery] Guid id)
        {
            try
            {
                return Ok(_employeeService.GetById(id));
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getAll")]
        public PaginatedEmployees GetAll([FromQuery] EmployeeFilter employeeFilter, [FromQuery] IPaginator paginator)
        {
            return _employeeService.GetAll(employeeFilter, paginator);
        }
    }
}