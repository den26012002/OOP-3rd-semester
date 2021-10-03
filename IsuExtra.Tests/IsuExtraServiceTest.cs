using System.Collections.Generic;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Services;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class Tests
    {
        [Test]
        [TestCase('M', "ФИТИП", "Функциональный анализ и математическая логика")]
        public void AddJGTA_JGTAAdded(char facultyLetter, string facultyName, string extraGroupName)
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty = new Faculty(facultyLetter, facultyName);
            JGTA extraGroup = isuExtraService.AddJGTA(faculty, extraGroupName);
            var extraGroups = new List<JGTA>(isuExtraService.ExtraGroups);
            if (!extraGroups.Contains(extraGroup))
            {
                Assert.Fail();
            }
        }

        [Test]
        public void AddStudentToJGTAStream_StudentAdded()
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty1 = isuService.AddFaculty('M', "ФИТИП");
            isuService.AddFaculty('R', "СУиР");
            var group = isuService.AddGroup("R3225");
            var groupTimeTable = new List<Lesson>();
            groupTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Monday,
                    new Time(10, 00),
                    new Time(11, 30))
                ));
            isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
            var student = isuService.AddStudent(group, "name");
            JGTA extraGroup = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
            JGTAStream stream = extraGroup.AddStream(25);
            var streamTimeTable = new List<Lesson>();
            streamTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Tuesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream, streamTimeTable);
            isuExtraService.AddStudentToJGTAStream(student, stream);
            var students = new List<Student>(stream.Students);
            if (!students.Contains(student))
            {
                Assert.Fail();
            }
        }

        [Test]
        public void AddStudentToJGTAStreamTimeTablesNotCompatible_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var isuService = new IsuService();
                var isuExtraService = new IsuExtraService(isuService);
                var faculty1 = isuService.AddFaculty('M', "ФИТИП");
                isuService.AddFaculty('R', "СУиР");
                var group = isuService.AddGroup("R3225");
                var groupTimeTable = new List<Lesson>();
                groupTimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Monday,
                        new Time(10, 00),
                        new Time(11, 30))
                    ));
                isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
                var student = isuService.AddStudent(group, "name");
                JGTA extraGroup = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
                JGTAStream stream = extraGroup.AddStream(25);
                var streamTimeTable = new List<Lesson>();
                streamTimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Monday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream, streamTimeTable);
                isuExtraService.AddStudentToJGTAStream(student, stream);
             });
        }

        [Test]
        public void AddStudentToMoreThanMaxNumberOfStreams_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var isuService = new IsuService();
                var isuExtraService = new IsuExtraService(isuService);
                var faculty1 = isuService.AddFaculty('M', "ФИТИП");
                var faculty2 = isuService.AddFaculty('R', "СУиР");
                var faculty3 = isuService.AddFaculty('X', "Some faculty");
                var group = isuService.AddGroup("M3200");
                var groupTimeTable = new List<Lesson>();
                groupTimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Monday,
                        new Time(10, 00),
                        new Time(11, 30))
                    ));
                isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
                
                JGTA extraGroup1 = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
                JGTAStream stream1 = extraGroup1.AddStream(25);
                var stream1TimeTable = new List<Lesson>();
                stream1TimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Tuesday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream1, stream1TimeTable);
                
                JGTA extraGroup2 = isuExtraService.AddJGTA(faculty2, "something");
                JGTAStream stream2 = extraGroup2.AddStream(25);
                var stream2TimeTable = new List<Lesson>();
                stream2TimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Wednesday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream2, stream2TimeTable);
                
                JGTA extraGroup3 = isuExtraService.AddJGTA(faculty3, "something");
                JGTAStream stream3 = extraGroup3.AddStream(25);
                var stream3TimeTable = new List<Lesson>();
                stream3TimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Friday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream3, stream3TimeTable);

                var student = isuService.AddStudent(group, "name");
                isuExtraService.AddStudentToJGTAStream(student, stream1);
                isuExtraService.AddStudentToJGTAStream(student, stream2);
                isuExtraService.AddStudentToJGTAStream(student, stream3);
            });
        }

        [Test]
        public void AddStudentToHisFacultyCourse_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
           {
               var isuService = new IsuService();
               var isuExtraService = new IsuExtraService(isuService);
               var faculty1 = isuService.AddFaculty('M', "ФИТИП");
               var group = isuService.AddGroup("M3200");
               var groupTimeTable = new List<Lesson>();
               groupTimeTable.Add(new Lesson(
                   new Teacher("teacher"),
                   new DayOfWeekTimePeriod(
                       DayOfWeek.Monday,
                       new Time(10, 00),
                       new Time(11, 30))
                   ));
               isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
               var student = isuService.AddStudent(group, "name");
               JGTA extraGroup = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
               JGTAStream stream = extraGroup.AddStream(25);
               var streamTimeTable = new List<Lesson>();
               streamTimeTable.Add(new Lesson(
                   new Teacher("teacher"),
                   new DayOfWeekTimePeriod(
                       DayOfWeek.Monday,
                       new Time(11, 00),
                       new Time(12, 30))
                   ));
               isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream, streamTimeTable);
               isuExtraService.AddStudentToJGTAStream(student, stream);
           });
        }

        [Test]
        public void AddStudentToOneJGTATwice_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty1 = isuService.AddFaculty('M', "ФИТИП");
            isuService.AddFaculty('R', "СУиР");
            var group = isuService.AddGroup("R3225");
            var groupTimeTable = new List<Lesson>();
            groupTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Monday,
                    new Time(10, 00),
                    new Time(11, 30))
                ));
            isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
            var student = isuService.AddStudent(group, "name");
            JGTA extraGroup = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
            JGTAStream stream1 = extraGroup.AddStream(25);
            var stream1TimeTable = new List<Lesson>();
            stream1TimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Tuesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream1, stream1TimeTable);

            JGTAStream stream2 = extraGroup.AddStream(20);
            var stream2TimeTable = new List<Lesson>();
            stream2TimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Wednesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream2, stream2TimeTable);
            isuExtraService.AddStudentToJGTAStream(student, stream1);
            isuExtraService.AddStudentToJGTAStream(student, stream2);
            });
        }

        [Test]
        public void RemoveStudentFromStream_StudentRemoved()
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty1 = isuService.AddFaculty('M', "ФИТИП");
            isuService.AddFaculty('R', "СУиР");
            var group = isuService.AddGroup("R3225");
            var groupTimeTable = new List<Lesson>();
            groupTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Monday,
                    new Time(10, 00),
                    new Time(11, 30))
                ));
            isuExtraService.TimeTablesService.AddGroupTimeTable(group, groupTimeTable);
            var student = isuService.AddStudent(group, "name");
            JGTA extraGroup = isuExtraService.AddJGTA(faculty1, "Функциональный анализ и математическая логика");
            JGTAStream stream = extraGroup.AddStream(25);
            var streamTimeTable = new List<Lesson>();
            streamTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Tuesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJGTAStreamTimeTable(stream, streamTimeTable);
            isuExtraService.AddStudentToJGTAStream(student, stream);
            isuExtraService.RemoveStudentFromJGTAStream(student, stream);
            var students = new List<Student>(stream.Students);
            if (students.Contains(student))
            {
                Assert.Fail();
            }
        }
    }
}
