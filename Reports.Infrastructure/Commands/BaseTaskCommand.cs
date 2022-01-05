using System;

namespace Reports.Infrastructure.Commands
{
    public abstract class BaseTaskCommand
    {
        protected BaseTaskCommand(Guid executorEmployeeId)
        {
            Id = Guid.NewGuid();
            ExecutorEmployeeId = executorEmployeeId;
        }

        internal BaseTaskCommand()
        {
        }

        public Guid Id { get; private init; }
        public Guid ExecutorEmployeeId { get; set; }
        public abstract void Execute();
    }
}
