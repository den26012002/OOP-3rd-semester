using System;

namespace Reports.Core.Entities
{
    public class ReportDateTimeInfo
    {
        public ReportDateTimeInfo(Report report, DateTime dateTime)
        {
            Id = Guid.NewGuid();
            Report = report;
            DateTime = dateTime;
        }

        private ReportDateTimeInfo()
        {
        }

        public Guid Id { get; private init; }
        public Report Report { get; private init; }
        public DateTime DateTime { get; private init; }
    }
}
