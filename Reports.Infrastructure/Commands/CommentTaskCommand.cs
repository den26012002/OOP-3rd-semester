using System;
using Reports.Core.Entities;
using Reports.Infrastructure.DAL;

namespace Reports.Infrastructure.Commands
{
    public class CommentTaskCommand : BaseTaskCommand
    {
        private ReportsContext _context;
        public CommentTaskCommand(Guid executorEmployeeId, ReportsContext context, Task commentedTask, TaskComment taskComment)
            : base(executorEmployeeId)
        {
            _context = context;
            CommentedTask = commentedTask;
            TaskComment = taskComment;
        }

        private CommentTaskCommand()
        {
        }

        public Task CommentedTask { get; private init; }
        public TaskComment TaskComment { get; private init; }

        public override void Execute()
        {
            CommentedTask.Comment(TaskComment);
            _context.TaskComments.Add(TaskComment);
            _context.SaveChanges();
        }
    }
}
