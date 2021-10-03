using System.Collections.Generic;
using Isu.Entities;

namespace IsuExtra.Entities
{
    public class Jgta // joint group of training areas
    {
        private List<JgtaStream> _streams;

        internal Jgta(Faculty faculty, string name)
        {
            Faculty = faculty;
            Name = name;
            _streams = new List<JgtaStream>();
        }

        public Faculty Faculty { get; }
        public string Name { get; }
        public IReadOnlyList<JgtaStream> Streams => _streams;

        public JgtaStream AddStream(int maxNumberOfStudents)
        {
            var newStream = new JgtaStream(Faculty, maxNumberOfStudents);
            _streams.Add(newStream);
            return newStream;
        }
    }
}
