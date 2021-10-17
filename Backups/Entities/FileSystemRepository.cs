using System;
using System.IO;

namespace Backups.Entities
{
    public class FileSystemRepository : IRepository
    {
        private string _backupsDirectoryPath;
        private IArchiver _archiver;
        private IStorageNameGiver _storageNameGiver;

        public FileSystemRepository(string backupsDirectoryPath, IArchiver archiver, IStorageNameGiver storageNameGiver)
        {
            _backupsDirectoryPath = backupsDirectoryPath;
            _archiver = archiver;
            _storageNameGiver = storageNameGiver;
        }

        public void SaveRestorePoint(RestorePoint restorePoint)
        {
            var backupsDirectory = new DirectoryInfo(_backupsDirectoryPath);
            if (!backupsDirectory.Exists)
            {
                backupsDirectory.Create();
            }

            DateTime dateTime = restorePoint.CreationDateTime;
            var restorePointDirectory = new DirectoryInfo(_backupsDirectoryPath +
                "\\" + restorePoint.Id + " (" + dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day + " " + dateTime.Hour + "-" + dateTime.Minute + "-" + dateTime.Second + ")");
            if (!restorePointDirectory.Exists)
            {
                restorePointDirectory.Create();
            }

            foreach (Storage storage in restorePoint.Storages)
            {
                _archiver.SaveStorage(storage, restorePointDirectory.FullName, _storageNameGiver.GiveName(storage) + "_" + restorePoint.Id.ToString());
            }
        }
    }
}
