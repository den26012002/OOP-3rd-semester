using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class Time
    {
        private readonly uint _maxHour = 23;
        private readonly uint _maxMinute = 59;
        private uint _hour;
        private uint _minute;
        public Time(uint hour, uint minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public uint Hour
        {
            get
            {
                return _hour;
            }
            private set
            {
                if (_hour > _maxHour)
                {
                    throw new IsuExtraException($"Error: unable to set number of hours more than {_maxHour}");
                }

                _hour = value;
            }
        }

        public uint Minute
        {
            get
            {
                return _minute;
            }
            private set
            {
                if (_minute > _maxMinute)
                {
                    throw new IsuExtraException($"Error: unable to set number of hours more than {_maxMinute}");
                }

                _minute = value;
            }
        }

        public static bool operator <(Time time1, Time time2)
        {
            if (time1.Hour < time2.Hour)
            {
                return true;
            }

            if (time1.Hour > time2.Hour)
            {
                return false;
            }

            return time1.Minute < time2.Minute;
        }

        public static bool operator >(Time time1, Time time2)
        {
            return time2 < time1;
        }
    }
}
