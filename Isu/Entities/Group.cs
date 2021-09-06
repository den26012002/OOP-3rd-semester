using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private string _name;
        private int _maxNumberOfStudents = 20;
        public Group(string name)
        {
            Name = name;
            Students = new List<Student>();
            Course = new CourseNumber(name[2] - '0');
        }

        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                if (value.Length != 5 && value.Length != 6)
                {
                    throw new IsuException("Error: incorrect name of group");
                }

                if (value[0] < 'A' || value[0] > 'Z')
                {
                    throw new IsuException("Error: incorrect name of group");
                }

                for (int i = 1; i < value.Length; ++i)
                {
                    if (value[i] < '0' || value[i] > '9')
                    {
                        throw new IsuException("Error: incorrect name of group");
                    }
                }

                _name = value;
            }
        }

        public List<Student> Students { get; }
        public CourseNumber Course { get; }
        public void AddStudent(Student student)
        {
            if (Students.Count + 1 > _maxNumberOfStudents)
            {
                throw new IsuException("Error: unable to add student to the group: too many students");
            }

            student.GroupName = Name;
            student.Course = Course;
            Students.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            Students.Remove(student);
            student.GroupName = null;
            student.Course = null;
        }

        public Student FindStudent(int id)
        {
            foreach (Student student in Students)
            {
                if (student.Id == id)
                {
                    return student;
                }
            }

            return null;
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in Students)
            {
                if (student.Name == name)
                {
                    return student;
                }
            }

            return null;
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
    }
}
