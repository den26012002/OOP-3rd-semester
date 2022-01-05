using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Core.Entities
{
    public class TaskComment
    {
        public TaskComment(Guid employeeId, string comment)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            Comment = comment;
        }

        private TaskComment()
        {
        }

        public Guid Id { get; private init; }
        public Guid EmployeeId { get; private init; }
        public string Comment { get; private init; }
    }
}
