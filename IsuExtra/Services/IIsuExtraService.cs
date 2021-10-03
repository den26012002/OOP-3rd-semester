using System.Collections.Generic;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface IIsuExtraService
    {
        public IIsuService IsuService { get; }
        public ITimeTablesService TimeTablesService { get; }
        public IReadOnlyList<JGTA> ExtraGroups { get; }

        public JGTA AddJGTA(Faculty faculty, string name);

        public void AddStudentToJGTAStream(Student student, JGTAStream stream);

        public void RemoveStudentFromJGTAStream(Student student, JGTAStream stream);

        public IReadOnlyList<JGTAStream> GetJGTAStreams(JGTA extraGroup);

        public IReadOnlyList<Student> GetStudentsOnStream(JGTAStream stream);

        public List<Student> GetNotSighUpStudents(Group group);
    }
}
