using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private readonly int _maxNumberOfStudents = 20;
        public Group(string name)
        {
            if (!IsValidName(name))
            {
                throw new IsuException("Error: incorrect name of group");
            }

            Name = name;
            Students = new List<Student>();
            Course = new CourseNumber((uint)(name[2] - '0'));
        }

        public string Name { get; }
        public List<Student> Students { get; }
        public CourseNumber Course { get; }
        public void AddStudent(Student student)
        {
            if (Students.Count + 1 > _maxNumberOfStudents)
            {
                throw new IsuException("Error: unable to add student to the group: too many students");
            }

            student.Group = this;
            Students.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            Students.Remove(student);
            student.Group = null;
        }

        public Student FindStudent(int id)
        {
            return Students.Find(student => student.Id == id);
        }

        public Student FindStudent(string name)
        {
            return Students.Find(student => student.Name == name);
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = new List<Student>();
            foreach (Student student in Students)
            {
                if (student.Course == courseNumber)
                {
                    students.Add(student);
                }
            }

            return students;
        }

        private bool IsValidName(string name)
        {
            if (name.Length != 5 && name.Length != 6)
            {
                return false;
            }

            if (name[0] < 'A' || name[0] > 'Z')
            {
                return false;
            }

            for (int i = 1; i < name.Length; ++i)
            {
                if (name[i] < '0' || name[i] > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
