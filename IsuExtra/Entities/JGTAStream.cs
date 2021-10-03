using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class JGTAStream
    {
        private int _maxNumberOfStudnents;
        private List<Student> _students;

        internal JGTAStream(Faculty faculty, int maxNumberOfStudents)
        {
            Faculty = faculty;
            _maxNumberOfStudnents = maxNumberOfStudents;
            _students = new List<Student>();
        }

        public Faculty Faculty { get; }
        public int FreePlacesNumber { get => _maxNumberOfStudnents - _students.Count; }
        public IReadOnlyList<Student> Students { get => _students; }

        internal void AddStudent(Student student)
        {
            if (Students.Count + 1 > _maxNumberOfStudnents)
            {
                throw new IsuExtraException("Error: unable to add student to the stream: too many students");
            }

            _students.Add(student);
        }

        internal void RemoveStudent(Student student)
        {
            if (!_students.Contains(student))
            {
                throw new IsuExtraException($"Error: student with id {student.Id} was not in stream");
            }

            _students.Remove(student);
        }
    }
}
