namespace IsuExtra.Entities
{
    public class DayOfWeekTimePeriod
    {
        public DayOfWeekTimePeriod(DayOfWeek dayOfWeek, Time startTime, Time finishTime)
        {
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            FinishTime = finishTime;
        }

        public DayOfWeek DayOfWeek { get; }
        public Time StartTime { get; }
        public Time FinishTime { get; }
    }
}