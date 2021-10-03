using System.Collections.Generic;

namespace IsuExtra.Entities
{
    public class JgtaStreamTimeTable
    {
        private List<Lesson> _timeTable;

        internal JgtaStreamTimeTable(JgtaStream stream, List<Lesson> timeTable)
        {
            JgtaStream = stream;
            _timeTable = timeTable;
        }

        public JgtaStream JgtaStream { get; }
        public IReadOnlyList<Lesson> TimeTable { get => _timeTable; }
    }
}
