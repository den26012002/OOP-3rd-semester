using System.IO;
using System.Text;

namespace BackupsExtra.Entities
{
    public class FileLogger : ILogger
    {
        private string _logFileName;

        public FileLogger(string logFileName)
        {
            _logFileName = logFileName;
            if (!File.Exists(_logFileName))
            {
                File.Create(_logFileName);
            }
        }

        public string LogFileName => _logFileName;
        public void Log(string message)
        {
            using (var ofstream = new FileStream(_logFileName, FileMode.Append))
            {
                ofstream.Write(Encoding.Default.GetBytes(message));
            }
        }
    }
}
