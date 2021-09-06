using System;
using System.Collections.Generic;
using System.Text;
using Isu.Entities;
using Isu.Tools;

namespace Isu.Services
{
    class IsuService : IIsuService
    {
        private int _nextId = 0;
        public List<Group> Groups { get; }
        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);
            Groups.Add(newGroup);
            return newGroup;
        }
        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _nextId++);
            group.AddStudent(newStudent);
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in Groups)
            {
                Student student = group.FindStudent(id);
                if (student != null)
                {
                    return student;
                }
            }

            throw new IsuException("");
        }

        public Student FindStudent(string name)
        {
            foreach (Group group in Groups)
            {
                Student student = group.FindStudent(name);
                if (student != null)
                {
                    return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in Groups)
            {
                if (group.Name == groupName)
                {
                    return group.Students;
                }
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = new List<Student>();
            foreach (Group group in Groups)
            { 
                students.AddRange(group.FindStudents())
            }
        }

        Group FindGroup(string groupName);
        List<Group> FindGroups(CourseNumber courseNumber);

        void ChangeStudentGroup(Student student, Group newGroup);
    }
}
