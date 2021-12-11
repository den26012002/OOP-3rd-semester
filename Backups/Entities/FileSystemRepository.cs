using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace Backups.Entities
{
    public class FileSystemRepository : IRepository
    {
        private string _backupsDirectoryPath;
        private IArchiver _archiver;
        private IStorageNameGiver _storageNameGiver;
        private List<RestorePointFileDirectoryInfo> _restorePointFileDirectoryInfos;

        public FileSystemRepository(string backupsDirectoryPath, IArchiver archiver, IStorageNameGiver storageNameGiver)
        {
            _backupsDirectoryPath = backupsDirectoryPath;
            _archiver = archiver;
            _storageNameGiver = storageNameGiver;
            _restorePointFileDirectoryInfos = new List<RestorePointFileDirectoryInfo>();
        }

        public string BackupsDirectoryPath => _backupsDirectoryPath;
        public IArchiver Archiver => _archiver;
        public IStorageNameGiver StorageNameGiver => _storageNameGiver;
        public IReadOnlyList<RestorePointFileDirectoryInfo> RestorePointFileDirectoryInfos => _restorePointFileDirectoryInfos;

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
            _restorePointFileDirectoryInfos.Add(new RestorePointFileDirectoryInfo(restorePoint.Id, restorePointDirectory.FullName));
            if (!restorePointDirectory.Exists)
            {
                restorePointDirectory.Create();
            }

            foreach (Storage storage in restorePoint.Storages)
            {
                _archiver.SaveStorage(storage, restorePointDirectory.FullName, _storageNameGiver.GiveName(storage) + "_" + restorePoint.Id.ToString());
            }
        }

        public void RemoveRestorePoint(uint restorePointId)
        {
            RestorePointFileDirectoryInfo restorePointFileDirectoryInfo = _restorePointFileDirectoryInfos.Find(info => info.RestorePointId == restorePointId);
            FileSystem.DeleteDirectory(
                restorePointFileDirectoryInfo.RestorePointDirectory,
                UIOption.AllDialogs,
                RecycleOption.SendToRecycleBin);
            _restorePointFileDirectoryInfos.Remove(restorePointFileDirectoryInfo);
        }

        public void MergeRestorePoints(uint oldRestorePointId, uint newRestorePointId)
        {
            DirectoryInfo oldDirectoryInfo = GetDirectoryInfo(oldRestorePointId);
            DirectoryInfo newDirectoryInfo = GetDirectoryInfo(newRestorePointId);

            FileInfo[] oldStoragesInfos = oldDirectoryInfo.GetFiles();
            FileInfo[] newStoragesInfos = newDirectoryInfo.GetFiles();
            foreach (FileInfo storageInfo in oldStoragesInfos)
            {
                if (!newStoragesInfos.Contains(storageInfo))
                {
                    storageInfo.MoveTo($"newDirectoryInfo.FullName\\{storageInfo.Name}");
                }
            }

            RemoveRestorePoint(oldRestorePointId);
        }

        public void ChangeBackupsDirectory(string newBackupsDirectoryPath)
        {
            _backupsDirectoryPath = newBackupsDirectoryPath;
        }

        public List<IJobObject> GetJobObjects(uint restorePointId)
        {
            List<Storage> storages = _archiver.LoadStorages(_restorePointFileDirectoryInfos.Find(info => info.RestorePointId == restorePointId).RestorePointDirectory);
            var jobObjects = new List<IJobObject>();
            foreach (Storage storage in storages)
            {
                jobObjects.AddRange(storage.JobObjects);
            }

            return jobObjects;
        }

        internal void UpdateRestorePointFileDirectoryInfos(List<RestorePointFileDirectoryInfo> restorePointFileDirectoryInfos)
        {
            _restorePointFileDirectoryInfos = restorePointFileDirectoryInfos;
        }

        private DirectoryInfo GetDirectoryInfo(uint restorePointId)
        {
            return new DirectoryInfo(_restorePointFileDirectoryInfos.Find(info => info.RestorePointId == restorePointId).RestorePointDirectory);
        }
    }
}
