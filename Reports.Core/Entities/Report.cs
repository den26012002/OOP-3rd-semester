using System;
using System.Collections.Generic;
using Reports.Core.Models;
using Reports.Core.Tools;

namespace Reports.Core.Entities
{
    public class Report
    {
        private List<Guid> _finishedTasksIds;

        public Report(Guid employeeId, string description = "")
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            State = ReportState.Open;
            Description = description;
            _finishedTasksIds = new List<Guid>();
        }

        private Report()
        {
        }

        public Guid Id { get; private init; }
        public Guid EmployeeId { get; private init; }
        public ReportState State { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyList<Guid> FinishedTasksIds => _finishedTasksIds;

        public void SetFinished()
        {
            if (State == ReportState.Finished)
            {
                throw new ReportsException("Error: report was already finished");
            }
            
            if (State == ReportState.Closed)
            {
                throw new ReportsException("Error: report was closed");
            }

            State = ReportState.Finished;
        }

        public void SetClosed()
        {
            if (State == ReportState.Open)
            {
                throw new ReportsException("Error: report wasn't finished");
            }

            if (State == ReportState.Closed)
            {
                throw new ReportsException("Error: report was already closed");
            }

            State = ReportState.Closed;
        }

        public void UpdateDescription(string newDescription)
        {
            if (State == ReportState.Closed)
            {
                throw new ReportsException("Error: report was closed");
            }

            Description = newDescription;
        }

        public void AddFinishedTaskId(Guid taskId)
        {
            if (State == ReportState.Closed)
            {
                throw new ReportsException("Error: report was closed");
            }

            _finishedTasksIds.Add(taskId);
        }
    }
}
