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
    [Route("/tasks")]
    public class TasksContoroller : ControllerBase
    {
        private ITasksService _tasksService;

        public TasksContoroller(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTask([FromQuery] Guid activeEmployeeId, [FromQuery] string taskDescription, [FromQuery] Guid? executorGuid)
        {
            try
            {
                return Ok(_tasksService.AddTask(activeEmployeeId, taskDescription, executorGuid));
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateTaskState([FromQuery] Guid activeEmployeeId, [FromQuery] Guid taskId, [FromQuery] TaskState newState)
        {
            try
            {
                _tasksService.UpdateTaskState(activeEmployeeId, taskId, newState);
                return Ok();
            }
            catch(ReportsException)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("changeExecutor")]
        public IActionResult ChangeTaskExecutor([FromQuery] Guid activeEmployeeId, [FromQuery] Guid taskId, [FromQuery] Guid newExecutorId)
        {
            try
            {
                _tasksService.ChangeTaskExecutor(activeEmployeeId, taskId, newExecutorId);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("comment")]
        public IActionResult CommentTask([FromQuery] Guid activeEmployeeId, [FromQuery] Guid taskId, [FromQuery] string comment)
        {
            try
            {
                _tasksService.CommentTask(activeEmployeeId, taskId, comment);
                return Ok();
            }
            catch (ReportsException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("findTask")]
        public Task FindTask([FromQuery] Guid id)
        {
            return _tasksService.FindTask(id);
        }

        [HttpGet]
        [Route("findTasks")]
        public IReadOnlyList<Task> FindTasks([FromQuery] DateTime creationalDate)
        {
            return _tasksService.FindTasks(creationalDate);
        }

        [HttpGet]
        [Route("findObtained")]
        public IReadOnlyList<Task> FindObtainedTasks([FromQuery] Guid employeeId)
        {
            return _tasksService.FindObtainedTasks(employeeId);
        }

        [HttpGet]
        [Route("findInferiors")]
        public IReadOnlyList<Task> FindInferiorsTasks([FromQuery] Guid employeeId)
        {
            return _tasksService.FindInferiorsTasks(employeeId);
        }

        [HttpGet]
        [Route("findChanged")]
        public IReadOnlyList<Task> FindChangedTasks([FromQuery] Guid employeeId)
        {
            return _tasksService.FindChangedTasks(employeeId);
        }
    }
}
