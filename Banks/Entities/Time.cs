using System;

namespace Banks.Entities
{
    public class Time
    {
        private DateTime _dateTime;

        public Time(DateTime startDateTime)
        {
            _dateTime = startDateTime;
        }

        public DateTime Now { get => _dateTime.Date; }
        public void Scroll(int numberOfDays)
        {
            _dateTime = _dateTime.AddDays(numberOfDays);
        }
    }
}
