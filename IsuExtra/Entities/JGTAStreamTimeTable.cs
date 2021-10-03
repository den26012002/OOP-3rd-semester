using System.Collections.Generic;

namespace IsuExtra.Entities
{
    public class JGTAStreamTimeTable
    {
        private List<Lesson> _timeTable;

        internal JGTAStreamTimeTable(JGTAStream stream, List<Lesson> timeTable)
        {
            JGTAStream = stream;
            _timeTable = timeTable;
        }

        public JGTAStream JGTAStream { get; }
        public IReadOnlyList<Lesson> TimeTable { get => _timeTable; }
    }
}
