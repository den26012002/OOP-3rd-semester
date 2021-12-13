using System;

namespace BackupsExtra.Entities
{
    public class LoggerTimeDecorator : ILogger
    {
        private ILogger _logger;

        public LoggerTimeDecorator(ILogger logger)
        {
            _logger = logger;
        }

        public ILogger OriginalLogger => _logger;

        public void Log(string message)
        {
            _logger.Log(DateTime.Now.ToString() + " " + message);
        }
    }
}
