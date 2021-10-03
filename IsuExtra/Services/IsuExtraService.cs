using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class IsuExtraService : IIsuExtraService
    {
        private readonly uint _maxNumberOfJgta = 2;
        private List<Jgta> _extraGroups;
        private JgtaCounter _jgtaCounter;

        public IsuExtraService(IIsuService isuService)
        {
            IsuService = isuService;
            _extraGroups = new List<Jgta>();
            TimeTablesService = new TimeTablesService();
            _jgtaCounter = new JgtaCounter(_maxNumberOfJgta);
        }

        public IIsuService IsuService { get; }
        public ITimeTablesService TimeTablesService { get; }
        public IReadOnlyList<Jgta> ExtraGroups => _extraGroups;

        public Jgta AddJgta(Faculty faculty, string name)
        {
            var newExtraGroup = new Jgta(faculty, name);
            _extraGroups.Add(newExtraGroup);
            return newExtraGroup;
        }

        public void AddStudentToJgtaStream(Student student, JgtaStream stream)
        {
            if (_jgtaCounter.GetNumberOfStudentJgta(student) >= _maxNumberOfJgta)
            {
                throw new IsuExtraException($"Error: unable to add student with id {student.Id} to more than {_maxNumberOfJgta} Jgta");
            }

            var faculties = new List<Faculty>(IsuService.Faculties);
            if (faculties.Find(faculty => faculty.Letter == student.Group.FacultyLetter).Name == stream.Faculty.Name)
            {
                throw new IsuExtraException($"Error: unable to add student with id {student.Id} to his faculty's Jgta");
            }

            Jgta extraGroup = _extraGroups.FirstOrDefault(existingExtraGroup => existingExtraGroup.Streams.Contains(stream));

            if (extraGroup == null)
            {
                throw new IsuExtraException("Error: stream is not exist");
            }

            foreach (JgtaStream existingStream in extraGroup.Streams)
            {
                var students = new List<Student>(existingStream.Students);
                if (students.Contains(student))
                {
                    throw new IsuExtraException($"Error: unable to add student with id {student.Id} to Jgta twice");
                }
            }

            if (stream.FreePlacesNumber < 0)
            {
                throw new IsuExtraException($"Error: not enough places on stream {stream}");
            }

            TimeTablesService.CheckStudentAndJgtaStreamCompatibility(student, stream, ExtraGroups);
            stream.AddStudent(student);
            _jgtaCounter.AddStudent(student);
        }

        public void RemoveStudentFromJgtaStream(Student student, JgtaStream stream)
        {
            stream.RemoveStudent(student);
            _jgtaCounter.RemoveStudent(student);
        }

        public IReadOnlyList<JgtaStream> GetJgtaStreams(Jgta extraGroup)
        {
            return extraGroup.Streams;
        }

        public IReadOnlyList<Student> GetStudentsOnStream(JgtaStream stream)
        {
            return stream.Students;
        }

        public List<Student> GetNotSighUpStudents(Group group)
        {
            var notSignUpStudents = new List<Student>();
            foreach (Student student in group.Students)
            {
                if (_jgtaCounter.GetNumberOfStudentJgta(student) == 0)
                {
                    notSignUpStudents.Add(student);
                }
            }

            return notSignUpStudents;
        }
    }
}
