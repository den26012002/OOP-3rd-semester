using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class TimeTablesService
    {
        private List<GroupTimeTable> _groupTimeTables;
        private List<JGTAStreamTimeTable> _streamTimeTables;
        internal TimeTablesService()
        {
            _groupTimeTables = new List<GroupTimeTable>();
            _streamTimeTables = new List<JGTAStreamTimeTable>();
        }

        public void AddGroupTimeTable(Group group, List<Lesson> groupTimeTable)
        {
            if (_groupTimeTables.Find(existingGroupTimeTable => existingGroupTimeTable.Group == group) != null)
            {
                throw new IsuExtraException($"Error: group with name {group.Name} already has timetable");
            }

            _groupTimeTables.Add(new GroupTimeTable(group, groupTimeTable));
        }

        public void AddJGTAStreamTimeTable(JGTAStream stream, List<Lesson> timeTable)
        {
            if (_streamTimeTables.Find(existingStreamTimeTable => existingStreamTimeTable.JGTAStream == stream) != null)
            {
                throw new IsuExtraException($"Error: stream {stream} already has timetable");
            }

            _streamTimeTables.Add(new JGTAStreamTimeTable(stream, timeTable));
        }

        public void CheckStudentAndJGTAStreamCompatibility(Student student, JGTAStream stream, IReadOnlyList<JGTA> allExtraGroups)
        {
            IReadOnlyList<Lesson> studentGroupTimeTable = _groupTimeTables.Find(groupTimeTable => groupTimeTable.Group == student.Group).TimeTable;
            if (studentGroupTimeTable == null)
            {
                throw new IsuExtraException($"Error: group with name {student.Group.Name} has no timetable");
            }

            IReadOnlyList<Lesson> streamTimeTable = _streamTimeTables.Find(streamTimeTable => streamTimeTable.JGTAStream == stream).TimeTable;
            if (studentGroupTimeTable == null)
            {
                throw new IsuExtraException($"Error: stream {stream} has no timetable");
            }

            var studentStreamsTimeTables = new List<IReadOnlyList<Lesson>>();

            foreach (JGTA extraGroup in allExtraGroups)
            {
                foreach (JGTAStream existingStream in extraGroup.Streams)
                {
                    var students = new List<Student>(existingStream.Students);
                    if (students.Contains(student))
                    {
                        studentStreamsTimeTables.Add(_streamTimeTables.Find(streamTimeTable => streamTimeTable.JGTAStream == existingStream).TimeTable);
                    }
                }
            }

            if (!IsTimeTablesCompatible(studentGroupTimeTable, streamTimeTable))
            {
                throw new IsuExtraException("Error: time tables are not compatible");
            }

            foreach (IReadOnlyList<Lesson> studentStreamTimeTable in studentStreamsTimeTables)
            {
                if (!IsTimeTablesCompatible(studentStreamTimeTable, streamTimeTable))
                {
                    throw new IsuExtraException("Error: time tables are not compatible");
                }
            }
        }

        private bool IsTimeTablesCompatible(IReadOnlyList<Lesson> timeTable1, IReadOnlyList<Lesson> timeTable2)
        {
            foreach (Lesson lesson1 in timeTable1)
            {
                foreach (Lesson lesson2 in timeTable2)
                {
                    if (!IsLessonsCompatible(lesson1, lesson2))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsLessonsCompatible(Lesson lesson1, Lesson lesson2)
        {
            if (lesson1.DayOfWeekTimePeriod.DayOfWeek != lesson2.DayOfWeekTimePeriod.DayOfWeek)
            {
                return true;
            }

            return (lesson1.DayOfWeekTimePeriod.StartTime > lesson2.DayOfWeekTimePeriod.StartTime &&
                lesson1.DayOfWeekTimePeriod.StartTime < lesson2.DayOfWeekTimePeriod.FinishTime) ||
                (lesson2.DayOfWeekTimePeriod.StartTime > lesson1.DayOfWeekTimePeriod.StartTime &&
                lesson2.DayOfWeekTimePeriod.StartTime < lesson1.DayOfWeekTimePeriod.FinishTime);
        }
    }
}
