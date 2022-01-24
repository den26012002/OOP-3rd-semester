using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Core.Entities;
using Reports.Core.Models;
using Reports.Core.Tools;
using Reports.Infrastructure.Commands;
using Reports.Infrastructure.DAL;

namespace Reports.Infrastructure.Services
{
    public class ReportsService : IReportsService
    {
        private ReportsContext _context;
        private IChronometer _chronometer;
        public ReportsService(ReportsContext context, IChronometer chronometer)
        {
            _context = context;
            _chronometer = chronometer;
        }
        public void AddTaskToReport(Guid activeEmployeeId, Guid taskId)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            if (!_context.Reports.Any(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished))))
            {
                throw new ReportsException("Error: report wasn't created");
            }

            Report employeeReport = _context.Reports.FirstOrDefault(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished)));
            employeeReport.AddFinishedTaskId(taskId);
            _context.SaveChanges();
        }

        public Report Create(Guid activeEmployeeId)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            if (_context.Reports.Any(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished))))
            {
                throw new ReportsException($"Error: opened report with EmployeeId={activeEmployeeId} has already exist");
            }

            var newReport = new Report(activeEmployeeId);
            _context.Reports.Add(newReport);
            _context.SaveChanges();
            return newReport;
        }

        public List<Guid> GetFinishedTasks(Guid activeEmployeeId)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            var finishCommands = _context.UpdateTaskCommands.Where(command => 
            (command.ExecutorEmployeeId == activeEmployeeId && command.NewState == TaskState.Resolved)).ToList();
            DateTime reportDateTime = _context.ReportDateTimeInfos.LastOrDefault(info => info.Report.EmployeeId == activeEmployeeId).DateTime;
            var finishedTasksIds = new List<Guid>();
            foreach (UpdateTaskCommand command in finishCommands)
            {
                DateTime executionDateTime =
                    _context.TaskCommandDateTimeInfos.FirstOrDefault(info => info.Command == command).ExecutionDateTime;
                if (executionDateTime >= reportDateTime)
                {
                    finishedTasksIds.Add(command.Task.Id);
                }
            }

            return finishedTasksIds;
        }

        public List<Report> GetInferiorsReports(Guid activeEmployeeId)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            var infeririors = _context.Employees.Where(employee => employee.BossId == activeEmployeeId).ToList();
            var inferiorsIds = new List<Guid>();
            foreach (Employee employee in infeririors)
            {
                inferiorsIds.Add(employee.Id);
            }

            return _context.Reports.Where(report => (inferiorsIds.Contains(report.EmployeeId) && report.State == ReportState.Finished)).ToList();
        }

        public List<Guid> GetInferiorsWithoutReports(Guid activeEmployeeId)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            var infeririors = _context.Employees.Where(employee => employee.BossId == activeEmployeeId).ToList();
            var inferiorsIds = new List<Guid>();
            foreach (Employee employee in infeririors)
            {
                if (_context.Reports.Any(report => (report.EmployeeId == employee.Id && report.State == ReportState.Finished)))
                {
                    inferiorsIds.Add(employee.Id);
                }
            }

            return inferiorsIds;
        }

        public void UpdateReportDesctiption(Guid activeEmployeeId, string newDescription)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            if (!_context.Reports.Any(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished))))
            {
                throw new ReportsException("Error: report wasn't created");
            }

            Report employeeReport = _context.Reports.FirstOrDefault(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished)));
            employeeReport.UpdateDescription(newDescription);
            _context.SaveChanges();
        }

        public void UpdateReportState(Guid activeEmployeeId, ReportState newState)
        {
            if (!_context.Employees.Any(employee => employee.Id == activeEmployeeId))
            {
                throw new ReportsException($"Error: employee with id {activeEmployeeId} doesn't exist");
            }

            if (!_context.Reports.Any(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished))))
            {
                throw new ReportsException("Error: report wasn't created");
            }

            Report employeeReport = _context.Reports.FirstOrDefault(
                report => (report.EmployeeId == activeEmployeeId && (report.State == ReportState.Open || report.State == ReportState.Finished)));
            if (newState == ReportState.Finished)
            {
                employeeReport.SetFinished();
            }
            
            if (newState == ReportState.Closed)
            {
                employeeReport.SetClosed();
                _context.ReportDateTimeInfos.Add(new ReportDateTimeInfo(employeeReport, _chronometer.GetDateTime()));
            }

            _context.SaveChanges();
        }
    }
}
