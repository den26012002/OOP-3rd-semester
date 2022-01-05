using System;
using Reports.Core.Entities;
using Reports.Infrastructure.DAL;

namespace Reports.Infrastructure.Commands
{
    public class AddTaskCommand : BaseTaskCommand
    {
        private ReportsContext _context;
        public AddTaskCommand(Guid executorEmpoyeeId, ReportsContext context, Task taskToAdd)
            : base(executorEmpoyeeId)
        {
            _context = context;
            TaskToAdd = taskToAdd;
        }

        private AddTaskCommand()
        {
        }

        public Task TaskToAdd { get; private init; }

        public override void Execute()
        {
            _context.Tasks.Add(TaskToAdd);
            _context.SaveChanges();
        }
    }
}
