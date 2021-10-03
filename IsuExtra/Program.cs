using System.Collections.Generic;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Services;

namespace IsuExtra
{
    internal class Program
    {
        private static void Main()
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            Faculty faculty1 = isuService.AddFaculty('M', "ФИТИП");
            isuService.AddFaculty('R', "СУиР");
            Group group = isuService.AddGroup("R3225");
            var groupTimeTable = new List<Lesson>();
            groupTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Monday,
                    new Time(10, 00),
                    new Time(11, 30))));
            isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
            Student student = isuService.AddStudent(group, "name");
            JGTA extraGroup = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
            JGTAStream stream = extraGroup.AddStream(25);
            var streamTimeTable = new List<Lesson>();
            groupTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Monday,
                    new Time(11, 00),
                    new Time(12, 30))));
            isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream, streamTimeTable);
            isuExtraService.AddStudentToJGTAStream(student, stream);
        }
    }
}
