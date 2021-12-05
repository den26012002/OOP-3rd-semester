/*using System.IO;
using Microsoft.VisualBasic.FileIO;*/
using Backups.Entities;
using BackupsExtra.Entities;

namespace BackupsExtra
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
            var logger = new ConsoleLogger();
            var restorePointsDeletingAlgorithm = new CountRestorePointsDeletingAlgorithm(5);
            var restorePointsReducer = new RestorePointsDeleter();
            var restorePointsRestorer = new FileSystemRestorePointsRestorer();
            var upgradedBackupJoba = new BackupJobaDeletingRestoringDecorator(backupJoba, restorePointsDeletingAlgorithm, restorePointsReducer, restorePointsRestorer);
            var loggeredUpgradedBackupJoba = new BackupJobaLogDecorator(upgradedBackupJoba, logger);
            var file1 = new FileObject("D:", "experimental", ".txt");
            var file2 = new FileObject("D:", "strange", ".txt");
            loggeredUpgradedBackupJoba.AddJobObject(file1);
            loggeredUpgradedBackupJoba.AddJobObject(file2);
            for (int i = 0; i < 7; ++i)
            {
                loggeredUpgradedBackupJoba.ProcessObjects();
            }

            var representer = new BackupJobaJsonRepresenter(loggeredUpgradedBackupJoba);
            System.Console.WriteLine(System.Text.Encoding.Default.GetString(representer.GetConfigurationRepresentation()));
            System.Console.WriteLine(System.Text.Encoding.Default.GetString(representer.GetSavedRestorePointsRepresentation()));
            upgradedBackupJoba.RestoreRestorePoint(3, "D:\\MyRestoredBackups");
        }
    }
}
