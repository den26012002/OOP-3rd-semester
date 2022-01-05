using System;
using Reports.Core.Entities;
using Reports.Core.Models;

namespace Reports.Infrastructure.Commands
{
    public class UpdateTaskCommand : BaseTaskCommand
    {
        public UpdateTaskCommand(Guid executorEmployeeId, Task task, TaskState newState)
            : base(executorEmployeeId)
        {
            Task = task;
            NewState = newState;
        }

        private UpdateTaskCommand()
        {
        }

        public Task Task { get; private init; }
        public TaskState NewState { get; private init; }

        public override void Execute()
        {
            if (NewState == TaskState.Active)
            {
                Task.SetActive();
            }

            if (NewState == TaskState.Resolved)
            {
                Task.SetResolved();
            }
        }
    }
}
