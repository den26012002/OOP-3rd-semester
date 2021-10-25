using System.Net;
using Backups.Server.Entities;

namespace Backups.Server
{
    internal class Program
    {
        private static void Main()
        {
            var server = new BackupTcpServer("D:\\MyBackups");
            server.Start(IPAddress.Parse("127.0.0.1"), 8888);
        }
    }
}
