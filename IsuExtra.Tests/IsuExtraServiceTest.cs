using System;
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
        [TestCase('M', "Fitip", "Functional analysis and mathematics logic")]
        public void AddJgta_JgtaAdded(char facultyLetter, string facultyName, string extraGroupName)
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty = new Faculty(facultyLetter, facultyName);
            Jgta extraGroup = isuExtraService.AddJgta(faculty, extraGroupName);
            var extraGroups = new List<Jgta>(isuExtraService.ExtraGroups);
            if (!extraGroups.Contains(extraGroup))
            {
                Assert.Fail();
            }
        }

        [Test]
        public void AddStudentToJgtaStream_StudentAdded()
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty1 = isuService.AddFaculty('M', "Fitip");
            isuService.AddFaculty('R', "Suir");
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
            Jgta extraGroup = isuExtraService.AddJgta(faculty1, "Functional analysis and mathematics logic");
            JgtaStream stream = extraGroup.AddStream(25);
            var streamTimeTable = new List<Lesson>();
            streamTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Tuesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream, streamTimeTable);
            isuExtraService.AddStudentToJgtaStream(student, stream);
            var students = new List<Student>(stream.Students);
            if (!students.Contains(student))
            {
                Assert.Fail();
            }
        }

        [Test]
        public void AddStudentToJgtaStreamTimeTablesNotCompatible_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var isuService = new IsuService();
                var isuExtraService = new IsuExtraService(isuService);
                var faculty1 = isuService.AddFaculty('M', "Fitip");
                isuService.AddFaculty('R', "Suir");
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
                Jgta extraGroup = isuExtraService.AddJgta(faculty1, "Functional analysis and mathematics logic");
                JgtaStream stream = extraGroup.AddStream(25);
                var streamTimeTable = new List<Lesson>();
                streamTimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Monday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream, streamTimeTable);
                isuExtraService.AddStudentToJgtaStream(student, stream);
             });
        }

        [Test]
        public void AddStudentToMoreThanMaxNumberOfStreams_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var isuService = new IsuService();
                var isuExtraService = new IsuExtraService(isuService);
                var faculty1 = isuService.AddFaculty('M', "Fitip");
                var faculty2 = isuService.AddFaculty('R', "Suir");
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
                
                Jgta extraGroup1 = isuExtraService.AddJgta(faculty1, "Functional analysis and mathematics logic");
                JgtaStream stream1 = extraGroup1.AddStream(25);
                var stream1TimeTable = new List<Lesson>();
                stream1TimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Tuesday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream1, stream1TimeTable);
                
                Jgta extraGroup2 = isuExtraService.AddJgta(faculty2, "something");
                JgtaStream stream2 = extraGroup2.AddStream(25);
                var stream2TimeTable = new List<Lesson>();
                stream2TimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Wednesday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream2, stream2TimeTable);
                
                Jgta extraGroup3 = isuExtraService.AddJgta(faculty3, "something");
                JgtaStream stream3 = extraGroup3.AddStream(25);
                var stream3TimeTable = new List<Lesson>();
                stream3TimeTable.Add(new Lesson(
                    new Teacher("teacher"),
                    new DayOfWeekTimePeriod(
                        DayOfWeek.Friday,
                        new Time(11, 00),
                        new Time(12, 30))
                    ));
                isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream3, stream3TimeTable);

                var student = isuService.AddStudent(group, "name");
                isuExtraService.AddStudentToJgtaStream(student, stream1);
                isuExtraService.AddStudentToJgtaStream(student, stream2);
                isuExtraService.AddStudentToJgtaStream(student, stream3);
            });
        }

        [Test]
        public void AddStudentToHisFacultyCourse_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
           {
               var isuService = new IsuService();
               var isuExtraService = new IsuExtraService(isuService);
               var faculty1 = isuService.AddFaculty('M', "Fitip");
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
               Jgta extraGroup = isuExtraService.AddJgta(faculty1, "Functional analysis and mathematics logic");
               JgtaStream stream = extraGroup.AddStream(25);
               var streamTimeTable = new List<Lesson>();
               streamTimeTable.Add(new Lesson(
                   new Teacher("teacher"),
                   new DayOfWeekTimePeriod(
                       DayOfWeek.Monday,
                       new Time(11, 00),
                       new Time(12, 30))
                   ));
               isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream, streamTimeTable);
               isuExtraService.AddStudentToJgtaStream(student, stream);
           });
        }

        [Test]
        public void AddStudentToOneJgtaTwice_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty1 = isuService.AddFaculty('M', "Fitip");
            isuService.AddFaculty('R', "Suir");
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
            Jgta extraGroup = isuExtraService.AddJgta(faculty1, "Functional analysis and mathematics logic");
            JgtaStream stream1 = extraGroup.AddStream(25);
            var stream1TimeTable = new List<Lesson>();
            stream1TimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Tuesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream1, stream1TimeTable);

            JgtaStream stream2 = extraGroup.AddStream(20);
            var stream2TimeTable = new List<Lesson>();
            stream2TimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Wednesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream2, stream2TimeTable);
            isuExtraService.AddStudentToJgtaStream(student, stream1);
            isuExtraService.AddStudentToJgtaStream(student, stream2);
            });
        }

        [Test]
        public void RemoveStudentFromStream_StudentRemoved()
        {
            var isuService = new IsuService();
            var isuExtraService = new IsuExtraService(isuService);
            var faculty1 = isuService.AddFaculty('M', "Fitip");
            isuService.AddFaculty('R', "Suir");
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
            Jgta extraGroup = isuExtraService.AddJgta(faculty1, "Functional analysis and mathematics logic");
            JgtaStream stream = extraGroup.AddStream(25);
            var streamTimeTable = new List<Lesson>();
            streamTimeTable.Add(new Lesson(
                new Teacher("teacher"),
                new DayOfWeekTimePeriod(
                    DayOfWeek.Tuesday,
                    new Time(11, 00),
                    new Time(12, 30))
                ));
            isuExtraService.TimeTablesService.AddJgtaStreamTimeTable(stream, streamTimeTable);
            isuExtraService.AddStudentToJgtaStream(student, stream);
            isuExtraService.RemoveStudentFromJgtaStream(student, stream);
            var students = new List<Student>(stream.Students);
            if (students.Contains(student))
            {
                Assert.Fail();
            }
        }
    }
}
