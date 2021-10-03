using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface ITimeTablesService
    {
        public IReadOnlyList<GroupTimeTable> GroupTimeTables { get; }
        public IReadOnlyList<JGTAStreamTimeTable> StreamTimeTables { get; }
        public void AddGroupTimeTable(Group group, List<Lesson> groupTimeTable);

        public void AddJGTAStreamTimeTable(JGTAStream stream, List<Lesson> timeTable);

        public void CheckStudentAndJGTAStreamCompatibility(Student student, JGTAStream stream, IReadOnlyList<JGTA> allExtraGroups);
    }
}
