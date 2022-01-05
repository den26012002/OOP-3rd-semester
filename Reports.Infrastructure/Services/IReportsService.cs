using System;
using System.Collections.Generic;
using Reports.Core.Entities;
using Reports.Core.Models;

namespace Reports.Infrastructure.Services
{
    public interface IReportsService
    {
        Report Create(Guid activeEmployeeId);
        List<Guid> GetFinishedTasks(Guid activeEmployeeId);
        List<Report> GetInferiorsReports(Guid activeEmployeeId);
        List<Guid> GetInferiorsWithoutReports(Guid activeEmployeeId);
        void AddTaskToReport(Guid activeEmployeeId, Guid taskId);
        void UpdateReportState(Guid activeEmployeeId, ReportState newState);
        void UpdateReportDesctiption(Guid activeEmployeeId, string newDescription);

    }
}
