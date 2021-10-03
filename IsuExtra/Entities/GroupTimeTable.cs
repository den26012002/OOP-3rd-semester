using System.Collections.Generic;
using Isu.Entities;

namespace IsuExtra.Entities
{
    public class GroupTimeTable
    {
        private List<Lesson> _timeTable;
        internal GroupTimeTable(Group group, List<Lesson> timeTable)
        {
            Group = group;
            _timeTable = timeTable;
        }

        public Group Group { get; }
        public IReadOnlyList<Lesson> TimeTable { get => _timeTable; }
    }
}
