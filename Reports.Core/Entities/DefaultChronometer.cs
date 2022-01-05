using System;

namespace Reports.Core.Entities
{
    public class DefaultChronometer : IChronometer
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
