using Isu.Services;
using Isu.Entities;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            //TODO: implement
            _isuService = null;
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            var isuService = new IsuService();
            isuService.AddFaculty('M', "‘»“Ëœ");
            Group group = isuService.AddGroup("M3200");
            Student student = isuService.AddStudent(group, "Attaboy");
            if (student.Group.Name != group.Name || group.FindStudent(student.Id) == null)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var isuService = new IsuService();
                Group group = isuService.AddGroup("M3200");
                for (int i = 0; i < 21; ++i)
                {
                    isuService.AddStudent(group, "Student" + i);
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var isuService = new IsuService();
                Group group = isuService.AddGroup("dsklafjasoiewjdsfoc");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var isuService = new IsuService();
            isuService.AddFaculty('M', "‘»“Ëœ");
            Group group = isuService.AddGroup("M3200");
            Student student = isuService.AddStudent(group, "Debtor");
            Group newGroup = isuService.AddGroup("M3212");
            isuService.ChangeStudentGroup(student, newGroup);
            if (student.Group.Name != newGroup.Name || newGroup.FindStudent(student.Id) == null)
            {
                Assert.Fail();
            }
        }
    }
}