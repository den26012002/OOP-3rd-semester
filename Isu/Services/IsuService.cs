using System.Collections.Generic;
using Isu.Entities;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private int _nextId = 0;

        public IsuService()
        {
            Groups = new List<Group>();
        }

        public List<Group> Groups { get; }
        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);
            Groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _nextId++, group.Name, group.Course);
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

            throw new IsuException("Error: student wasn't finded");
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
                if (group.Course == courseNumber)
                {
                    students.AddRange(group.Students);
                }
            }

            return students;
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in Groups)
            {
                if (group.Name == groupName)
                {
                    return group;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = new List<Group>();
            foreach (Group group in Groups)
            {
                if (group.Course == courseNumber)
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            FindGroup(student.GroupName).RemoveStudent(student);
            newGroup.AddStudent(student);
        }
    }
}
