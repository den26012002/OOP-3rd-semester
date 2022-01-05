using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Reports.Infrastructure.Services;
using Reports.Core.Entities;
using Reports.Core.Models;
using Reports.Core.Tools;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/reports")]
    public class ReportsController : ControllerBase
    {
        private IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromQuery] Guid activeEmployeeId)
        {
            try
            {
                return Ok(_reportsService.Create(activeEmployeeId));
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getTasks")]
        public IActionResult GetFinishedTasks([FromQuery] Guid activeEmployeeId)
        {
            try
            {
                return Ok(_reportsService.GetFinishedTasks(activeEmployeeId));
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getInferiorsReports")]
        public IActionResult GetInferiorsReports([FromQuery] Guid activeEmployeeId)
        {
            try
            {
                return Ok(_reportsService.GetInferiorsReports(activeEmployeeId));
            }
            catch (ReportsException) 
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getInferiorsWithoutReports")]
        public IActionResult GetInferiorsWithoutReports([FromQuery] Guid activeEmployeeId)
        {
            try
            {
                return Ok(_reportsService.GetInferiorsWithoutReports(activeEmployeeId));
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("addTask")]
        public IActionResult AddTaskToReport([FromQuery] Guid activeEmployeeId, [FromQuery] Guid taskId)
        {
            try
            {
                _reportsService.AddTaskToReport(activeEmployeeId, taskId);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("updateState")]
        public IActionResult UpdateReportState([FromQuery] Guid activeEmployeeId, [FromQuery] ReportState newState)
        {
            try
            {
                _reportsService.UpdateReportState(activeEmployeeId, newState);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("updateDescription")]
        public IActionResult UpdateReportDesctiption([FromQuery] Guid activeEmployeeId, [FromQuery] string newDescription)
        {
            try
            {
                _reportsService.UpdateReportDesctiption(activeEmployeeId, newDescription);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }
    }
}