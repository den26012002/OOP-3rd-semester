using System.Collections.Generic;
using Isu.Entities;

namespace IsuExtra.Entities
{
    public class JGTA // joint group of training areas
    {
        private List<JGTAStream> _streams;

        internal JGTA(Faculty faculty)
        {
            Faculty = faculty;
            _streams = new List<JGTAStream>();
        }

        public Faculty Faculty { get; }
        public IReadOnlyList<JGTAStream> Streams { get => _streams; }

        public JGTAStream AddStream(int maxNumberOfStudents)
        {
            var newStream = new JGTAStream(Faculty, maxNumberOfStudents);
            _streams.Add(newStream);
            return newStream;
        }
    }
}
