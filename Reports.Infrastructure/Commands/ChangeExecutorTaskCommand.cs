using System;
using Reports.Core.Entities;

namespace Reports.Infrastructure.Commands
{
    public class ChangeExecutorTaskCommand : BaseTaskCommand
    {
        public ChangeExecutorTaskCommand(Guid executorEmployeeId, Task task, Guid newExecutorId)
            : base(executorEmployeeId)
        {
            Task = task;
            NewExecutorId = newExecutorId;
        }

        private ChangeExecutorTaskCommand()
        {
        }

        public Task Task { get; private init; }
        public Guid NewExecutorId { get; private init; }

        public override void Execute()
        {
            Task.ChangeExecutor(NewExecutorId);
        }
    }
}
