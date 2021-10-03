using System.Collections.Generic;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class IsuExtraService : IIsuExtraService
    {
        private readonly uint _maxNumberOfJGTA = 2;
        private List<JGTA> _extraGroups;
        private Dictionary<Student, uint> _numberOfStudentJGTA;

        public IsuExtraService(IIsuService isuService)
        {
            IsuService = isuService;
            _extraGroups = new List<JGTA>();
            TimeTablesService = new TimeTablesService();
            _numberOfStudentJGTA = new Dictionary<Student, uint>();
        }

        public IIsuService IsuService { get; }
        public ITimeTablesService TimeTablesService { get; }
        public IReadOnlyList<JGTA> ExtraGroups { get; }

        public JGTA AddJGTA(Faculty faculty, string name)
        {
            var newExtraGroup = new JGTA(faculty, name);
            _extraGroups.Add(newExtraGroup);
            return newExtraGroup;
        }

        public void AddStudentToJGTAStream(Student student, JGTAStream stream)
        {
            if (_numberOfStudentJGTA[student] >= _maxNumberOfJGTA)
            {
                throw new IsuExtraException($"Error: unable to add student with id {student.Id} to more than {_maxNumberOfJGTA} JGTA");
            }

            var faculties = new List<Faculty>(IsuService.Faculties);
            if (faculties.Find(faculty => faculty.Letter == student.Group.FacultyLetter).Name == stream.Faculty.Name)
            {
                throw new IsuExtraException($"Error: unable to add student with id {student.Id} to his faculty's JGTA");
            }

            JGTA extraGroup = null;
            foreach (JGTA existingExtraGroup in _extraGroups)
            {
                var streams = new List<JGTAStream>(existingExtraGroup.Streams);
                if (streams.Contains(stream))
                {
                    extraGroup = existingExtraGroup;
                    break;
                }
            }

            if (extraGroup == null)
            {
                throw new IsuExtraException("Error: stream is not exist");
            }

            foreach (JGTAStream existingStream in extraGroup.Streams)
            {
                var students = new List<Student>(existingStream.Students);
                if (students.Contains(student))
                {
                    throw new IsuExtraException($"Error: unable to add student with id {student.Id} to JGTA twice");
                }
            }

            if (stream.FreePlacesNumber > 0)
            {
                TimeTablesService.CheckStudentAndJGTAStreamCompatibility(student, stream, ExtraGroups);
                stream.AddStudent(student);
                if (!_numberOfStudentJGTA.ContainsKey(student))
                {
                    _numberOfStudentJGTA[student] = 1;
                }
                else
                {
                    ++_numberOfStudentJGTA[student];
                }
            }
        }

        public void RemoveStudentFromJGTAStream(Student student, JGTAStream stream)
        {
            stream.RemoveStudent(student);
            --_numberOfStudentJGTA[student];
        }

        public IReadOnlyList<JGTAStream> GetJGTAStreams(JGTA extraGroup)
        {
            return extraGroup.Streams;
        }

        public IReadOnlyList<Student> GetStudentsOnStream(JGTAStream stream)
        {
            return stream.Students;
        }

        public List<Student> GetNotSighUpStudents(Group group)
        {
            var notSignUpStudents = new List<Student>();
            foreach (Student student in group.Students)
            {
                bool isSignUp = false;
                foreach (JGTA extraGroup in _extraGroups)
                {
                    foreach (JGTAStream stream in extraGroup.Streams)
                    {
                        var students = new List<Student>(stream.Students);
                        if (students.Contains(student))
                        {
                            isSignUp = true;
                        }
                    }
                }

                if (!isSignUp)
                {
                    notSignUpStudents.Add(student);
                }
            }

            return notSignUpStudents;
        }
    }
}
