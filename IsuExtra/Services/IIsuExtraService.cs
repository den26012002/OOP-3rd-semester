using System.Collections.Generic;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface IIsuExtraService
    {
        IIsuService IsuService { get; }
        ITimeTablesService TimeTablesService { get; }
        IReadOnlyList<Jgta> ExtraGroups { get; }

        Jgta AddJgta(Faculty faculty, string name);

        void AddStudentToJgtaStream(Student student, JgtaStream stream);

        void RemoveStudentFromJgtaStream(Student student, JgtaStream stream);

        IReadOnlyList<JgtaStream> GetJgtaStreams(Jgta extraGroup);

        IReadOnlyList<Student> GetStudentsOnStream(JgtaStream stream);

        List<Student> GetNotSighUpStudents(Group group);
    }
}
