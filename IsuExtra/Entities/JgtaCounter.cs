using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    internal class JgtaCounter
    {
        private uint _maxNumberOfJgta;
        private Dictionary<Student, uint> _numberOfStudentJgta;
        internal JgtaCounter(uint maxNumberOfJgta)
        {
            _maxNumberOfJgta = maxNumberOfJgta;
            _numberOfStudentJgta = new Dictionary<Student, uint>();
        }

        internal void AddStudent(Student student)
        {
            if (!_numberOfStudentJgta.ContainsKey(student))
            {
                _numberOfStudentJgta[student] = 1;
            }
            else
            {
                ++_numberOfStudentJgta[student];
            }
        }

        internal void RemoveStudent(Student student)
        {
            if (!_numberOfStudentJgta.ContainsKey(student) || _numberOfStudentJgta[student] == 0)
            {
                throw new IsuExtraException($"Error: number of Jgta of student with id {student.Id} can't be less than 0");
            }

            --_numberOfStudentJgta[student];
        }

        internal uint GetNumberOfStudentJgta(Student student)
        {
            if (!_numberOfStudentJgta.ContainsKey(student))
            {
                return 0;
            }

            return _numberOfStudentJgta[student];
        }
    }
}
