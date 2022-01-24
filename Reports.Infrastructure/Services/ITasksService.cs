using System;
using System.Collections.Generic;
using Reports.Core.Entities;
using Reports.Core.Models;

namespace Reports.Infrastructure.Services
{
    public interface ITasksService
    {
        Task FindTask(Guid taskId);
        IReadOnlyList<Task> FindTasks(DateTime creationalDate);
        IReadOnlyList<Task> FindObtainedTasks(Guid employeeId);
        Task AddTask(Guid activeEmployeeId, string taskDescription, Guid? executorId = null);
        void UpdateTaskState(Guid activeEmployeeId, Guid taskId, TaskState newState);
        void CommentTask(Guid activeEmployeeId, Guid taskId, string comment);
        void ChangeTaskExecutor(Guid activeEmployeeId, Guid taskId, Guid newExecutorId);
        IReadOnlyList<Task> FindInferiorsTasks(Guid employeeId);
        IReadOnlyList<Task> FindChangedTasks(Guid employeeId);
    }
}
