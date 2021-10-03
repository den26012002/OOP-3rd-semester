using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface ITimeTablesService
    {
        IReadOnlyList<GroupTimeTable> GroupTimeTables { get; }
        IReadOnlyList<JgtaStreamTimeTable> StreamTimeTables { get; }
        void AddGroupTimeTable(Group group, List<Lesson> groupTimeTable);

        void AddJgtaStreamTimeTable(JgtaStream stream, List<Lesson> timeTable);

        void CheckStudentAndJgtaStreamCompatibility(Student student, JgtaStream stream, IReadOnlyList<Jgta> allExtraGroups);
    }
}
