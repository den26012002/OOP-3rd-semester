namespace Isu.Entities
{
    public class Student
    {
        public Student(string name, int id, string groupName, CourseNumber courseNumber)
        {
            Name = name;
            Id = id;
            GroupName = groupName;
            Course = courseNumber;
        }

        public int Id { get; }
        public string Name { get; }
        public string GroupName { get; set; }
        public CourseNumber Course { get; set; }
    }
}
