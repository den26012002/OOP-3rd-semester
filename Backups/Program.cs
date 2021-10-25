using Backups.Entities;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var singleStorageAlgorithm = new SingleStorageAlgorithm();
            var zipArchiver = new ZipArchiver();
            var defaultStorageNameGiver = new DefaultStorageNameGiver();
            var fileSystemRepository = new FileSystemRepository("D:\\MyBackupsStorages", zipArchiver, defaultStorageNameGiver);
            var backupJoba = new BackupJoba("Backup", singleStorageAlgorithm, fileSystemRepository);
            var file1 = new FileObject("D:", "experimental", ".txt");
            var file2 = new FileObject("D:", "strange", ".txt");
            backupJoba.AddJobObject(file1);
            backupJoba.AddJobObject(file2);
            backupJoba.ProcessObjects();
        }
    }
}
