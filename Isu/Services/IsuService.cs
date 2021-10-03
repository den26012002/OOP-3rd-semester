using System.Collections.Generic;
using Isu.Entities;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private int _nextId = 0;
        private List<Group> _groups;
        private List<Faculty> _faculties;

        public IsuService()
        {
            _groups = new List<Group>();
            _faculties = new List<Faculty>();
        }

        public IReadOnlyList<Group> Groups { get => _groups; }
        public IReadOnlyList<Faculty> Faculties { get => _faculties; }
        public Group AddGroup(string name)
        {
            if (!IsValidFaculty(name[0]))
            {
                throw new IsuException("Error: incorrect letter of faculty");
            }

            var newGroup = new Group(name);
            _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _nextId++, group);
            group.AddStudent(newStudent);
            return newStudent;
        }

        public Faculty AddFaculty(char letter, string name)
        {
            var newFaculty = new Faculty(letter, name);
            _faculties.Add(newFaculty);
            return newFaculty;
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
                    return new List<Student>(group.Students);
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
            FindGroup(student.Group.Name).RemoveStudent(student);
            newGroup.AddStudent(student);
        }

        private bool IsValidFaculty(char letter)
        {
            return _faculties.Find(faculty => faculty.Letter == letter) != null;
        }
    }
}
