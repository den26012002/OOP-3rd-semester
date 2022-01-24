using System;
using Reports.Core.Entities;

namespace Reports.Infrastructure.Commands
{
    public class TaskCommandDateTimeInfo
    {
        public TaskCommandDateTimeInfo(BaseTaskCommand command, DateTime executionDateTime)
        {
            Id = Guid.NewGuid();
            Command = command;
            ExecutionDateTime = executionDateTime;
        }

        private TaskCommandDateTimeInfo()
        {
        }

        public Guid Id { get; private init; }
        public BaseTaskCommand Command { get; private init; }
        public DateTime ExecutionDateTime { get; private init; }
    }
}
