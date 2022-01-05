using System;
using System.Collections.Generic;
using Reports.Core.Models;
using Reports.Core.Tools;

namespace Reports.Core.Entities
{
    public class Task
    {
        private List<TaskComment> _taskComments;

        public Task(string description, Guid? executorId = null)
        {
            Id = Guid.NewGuid();
            State = TaskState.Open;
            Description = description;
            ExecutorId = executorId;
            _taskComments = new List<TaskComment>();
        }

        private Task()
        {
        }

        public Guid Id { get; private init; }
        public TaskState State { get; private set; }
        public string Description { get; private init; }
        public Guid? ExecutorId { get; private set; }
        public IReadOnlyList<TaskComment> TaskComments => _taskComments;

        public void SetActive()
        {
            if (State != TaskState.Open)
            {
                throw new ReportsException("Error: task was already marked as active");
            }
            State = TaskState.Active;
        }

        public void SetResolved()
        {
            if (State == TaskState.Open)
            {
                throw new ReportsException("Error: task wasn't marked as active");
            }

            if (State == TaskState.Resolved)
            {
                throw new ReportsException("Error: task was already marked as resolved");
            }

            State = TaskState.Resolved;
        }
        public void ChangeExecutor(Guid newExecutorId)
        {
            ExecutorId = newExecutorId;
        }
        public void Comment(TaskComment comment)
        {
            _taskComments.Add(comment);
        }
    }
}
