namespace Isu.Entities
{
    public class Student
    {
        public Student(string name, int id, Group group)
        {
            Name = name;
            Id = id;
            Group = group;
        }

        public int Id { get; }
        public string Name { get; }
        public Group Group { get; set; }
        public CourseNumber Course { get => Group.Course; }
    }
}
