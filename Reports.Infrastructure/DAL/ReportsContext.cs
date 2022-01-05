using Microsoft.EntityFrameworkCore;
using Reports.Core.Entities;
using Reports.Infrastructure.Commands;

namespace Reports.Infrastructure.DAL
{
    public class ReportsContext : DbContext
    {
        public ReportsContext(DbContextOptions<ReportsContext> dbContextOptions) :
            base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; private set; }
        public DbSet<Task> Tasks { get; private set; }
        public DbSet<TaskComment> TaskComments { get; private set; }
        public DbSet<TaskCommandDateTimeInfo> TaskCommandDateTimeInfos { get; private set; }
        public DbSet<BaseTaskCommand> TaskCommands { get; private set; }
        public DbSet<AddTaskCommand> AddTaskCommands { get; private set; }
        public DbSet<ChangeExecutorTaskCommand> ChangeExecutorTaskCommands { get; private set; }
        public DbSet<CommentTaskCommand> CommentTaskCommands { get; private set; }
        public DbSet<UpdateTaskCommand> UpdateTaskCommands { get; private set; }
        public DbSet<Report> Reports { get; private set; }
        public DbSet<ReportDateTimeInfo> ReportDateTimeInfos { get; private set; }
    }
}
