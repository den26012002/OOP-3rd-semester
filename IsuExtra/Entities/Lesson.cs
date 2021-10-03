namespace IsuExtra.Entities
{
    public class Lesson
    {
        public Lesson(Teacher teacher, DayOfWeekTimePeriod dayOfWeekTimePeriod)
        {
            Teacher = teacher;
            DayOfWeekTimePeriod = dayOfWeekTimePeriod;
        }

        public Teacher Teacher { get; }
        public DayOfWeekTimePeriod DayOfWeekTimePeriod { get; }
    }
}
