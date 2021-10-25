using Backups.Client.Entities;
using Backups.Entities;

namespace Backups.Client
{
    internal class Program
    {
        private static void Main()
        {
            var splitStoragesAlgorithm = new SplitStoragesAlgorithm();
            var repository = new FileSystemRepository(string.Empty, new ZipArchiver(), new DefaultStorageNameGiver());
            var backupJoba = new BackupJoba("ForClientServer", splitStoragesAlgorithm, repository);
            var client = new BackupsTcpClient("127.0.0.1", 8888, backupJoba);
            client.SendConfiguration();
            client.SendAddJobObject(new FileObject("D:\\", "experimental", ".txt"));
            client.SendAddJobObject(new FileObject("D:\\", "strange", ".txt"));
            client.SendProcessObjects();
        }
    }
}
