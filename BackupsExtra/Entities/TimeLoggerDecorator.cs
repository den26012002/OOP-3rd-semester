using System;

namespace BackupsExtra.Entities
{
    public class TimeLoggerDecorator : ILogger
    {
        private ILogger _logger;

        public TimeLoggerDecorator(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger.Log(DateTime.Now.ToString() + " " + message);
        }
    }
}
