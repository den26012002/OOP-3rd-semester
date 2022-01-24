using Reports.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Reports.Core.Entities;
using Reports.Core.Tools;
using Reports.Infrastructure.DAL;
using Reports.Infrastructure.Commands;

namespace Reports.Infrastructure.Services
{
    public class TasksService : ITasksService
    {
        private ReportsContext _context;
        private IChronometer _chronometer;

        public TasksService(ReportsContext context, IChronometer chronometer)
        {
            _context = context;
            _chronometer = chronometer;
        }

        public Task AddTask(Guid activeEmployeeId, string taskDescription, Guid? executorId = null)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            var newTask = new Task(taskDescription, executorId);
            var addTaskCommand = new AddTaskCommand(activeEmployeeId, _context, newTask);
            addTaskCommand.Execute();
            _context.AddTaskCommands.Add(addTaskCommand);
            _context.TaskCommandDateTimeInfos.Add(new TaskCommandDateTimeInfo(addTaskCommand, _chronometer.GetDateTime()));
            _context.SaveChanges();
            return newTask;
        }

        public void ChangeTaskExecutor(Guid activeEmployeeId, Guid taskId, Guid newExecutorId)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            var changeExecutorCommand = new ChangeExecutorTaskCommand(activeEmployeeId, FindTask(taskId), newExecutorId);
            changeExecutorCommand.Execute();
            _context.ChangeExecutorTaskCommands.Add(changeExecutorCommand);
            _context.TaskCommandDateTimeInfos.Add(new TaskCommandDateTimeInfo(changeExecutorCommand, _chronometer.GetDateTime()));
            _context.SaveChanges();
        }

        public void CommentTask(Guid activeEmployeeId, Guid taskId, string comment)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            if (!_context.Tasks.Any(task => task.Id == taskId))
            {
                throw new ReportsException($"Error: task with id {taskId} doesn't exist");
            }

            var commentTaskCommand = new CommentTaskCommand(activeEmployeeId, _context, FindTask(taskId), new TaskComment(activeEmployeeId, comment));
            commentTaskCommand.Execute();
            _context.CommentTaskCommands.Add(commentTaskCommand);
            _context.TaskCommandDateTimeInfos.Add(new TaskCommandDateTimeInfo(commentTaskCommand, _chronometer.GetDateTime()));
            _context.Tasks.Update(FindTask(taskId));
            _context.SaveChanges();
        }

        public IReadOnlyList<Task> FindChangedTasks(Guid employeeId)
        {
            var addTaskCommands = _context.AddTaskCommands.Include(command => command.TaskToAdd.TaskComments).Where(taskCommand => taskCommand.ExecutorEmployeeId == employeeId).ToList();
            var updateTaskCommands = _context.UpdateTaskCommands.Where(taskCommand => taskCommand.ExecutorEmployeeId == employeeId).ToList();
            var commentTaskCommands = _context.CommentTaskCommands.Where(taskCommand => taskCommand.ExecutorEmployeeId == employeeId).ToList();
            var changeExecutorTaskCommands = _context.ChangeExecutorTaskCommands.Where(taskCommand => taskCommand.ExecutorEmployeeId == employeeId).ToList();
            var changedTasks = new List<Task>();
            foreach (AddTaskCommand addTaskCommand in addTaskCommands)
            {
                changedTasks.Add(addTaskCommand.TaskToAdd);
            }

            foreach (UpdateTaskCommand updateTaskCommand in updateTaskCommands)
            {
                changedTasks.Add(updateTaskCommand.Task);
            }

            foreach (CommentTaskCommand commentTaskCommand in commentTaskCommands)
            {
                changedTasks.Add(commentTaskCommand.CommentedTask);
            }

            foreach (ChangeExecutorTaskCommand changeExecutorTaskCommand in changeExecutorTaskCommands)
            {
                changedTasks.Add(changeExecutorTaskCommand.Task);
            }

            return changedTasks;
        }

        public IReadOnlyList<Task> FindInferiorsTasks(Guid employeeId)
        {
            var inferiors = _context.Employees.Where(employee => employee.BossId == employeeId).ToList();
            var inferiorsTasks = new List<Task>();
            foreach (Employee employee in inferiors)
            {
                inferiorsTasks.AddRange(FindObtainedTasks(employee.Id));
            }
            return inferiorsTasks;
        }

        public IReadOnlyList<Task> FindObtainedTasks(Guid employeeId)
        {
            return _context.Tasks.Where(task => task.ExecutorId == employeeId).ToList();
        }

        public Task FindTask(Guid taskId)
        {
            return _context.Tasks.Include(task => task.TaskComments).FirstOrDefault(task => task.Id == taskId);
        }

        public IReadOnlyList<Task> FindTasks(DateTime creationalDate)
        {
            var infos = _context.TaskCommandDateTimeInfos.Where(info => info.ExecutionDateTime.Date == creationalDate).ToList();
            var addTaskCommands = new List<AddTaskCommand>();
            foreach (TaskCommandDateTimeInfo info in infos)
            {
                if (info.Command is AddTaskCommand addTaskCommand)
                {
                    addTaskCommands.Add(addTaskCommand);
                }
            }
            var tasks = new List<Task>();
            foreach (AddTaskCommand addTaskCommand in addTaskCommands)
            {
                tasks.Add(addTaskCommand.TaskToAdd);
            }
            return tasks;
        }

        public void UpdateTaskState(Guid activeEmployeeId, Guid taskId, TaskState newState)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            if (!_context.Tasks.Any(task => task.Id == taskId))
            {
                throw new ReportsException($"Error: task with id {taskId} doesn't exist");
            }

            var updateTaskCommand = new UpdateTaskCommand(activeEmployeeId, FindTask(taskId), newState);
            updateTaskCommand.Execute();
            _context.UpdateTaskCommands.Add(updateTaskCommand);
            _context.TaskCommandDateTimeInfos.Add(new TaskCommandDateTimeInfo(updateTaskCommand, _chronometer.GetDateTime()));
            _context.SaveChanges();
        }
    }
}
