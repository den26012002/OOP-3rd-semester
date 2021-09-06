using System;
using System.Collections.Generic;
using System.Text;

namespace Isu.Entities
{
    public class Group
    {
        public Group(string name)
        {
            Name = name;
            Students = new List<Student>();
        }

        public string Name { get; }
        public List<Student> Students { get; }
        public void AddStudent(Student student)
        {
            Students.Add(student);
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
