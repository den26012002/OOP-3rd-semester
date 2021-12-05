using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobaLogDecorator : IBackupJoba
    {
        private IBackupJoba _backupJoba;
        private ILogger _logger;
        public BackupJobaLogDecorator(
            IBackupJoba backupJoba,
            ILogger logger)
        {
            _backupJoba = new BackupJoba(
                backupJoba.Name,
                new StorageAlgorithmLogDecorator(backupJoba.StorageAlgorithm, logger),
                new RepositoryLogDecorator(backupJoba.Repository, logger));
            _logger = logger;
        }

        public string Name => _backupJoba.Name;

        public IReadOnlyList<IJobObject> CurrentObjects => _backupJoba.CurrentObjects;

        public IStorageAlgorithm StorageAlgorithm => _backupJoba.StorageAlgorithm;

        public IRepository Repository => _backupJoba.Repository;

        public List<RestorePointJobObjectsInfo> CreatedRestorePoints => _backupJoba.CreatedRestorePoints;

        public IBackupJoba OriginalBackupJoba => _backupJoba;
        public ILogger Logger => _logger;

        public void AddJobObject(IJobObject jobObject)
        {
            _logger.Log($"Start adding a new job object with name {jobObject.Name}...");
            _backupJoba.AddJobObject(jobObject);
            _logger.Log($"Job object with name {jobObject.Name} added successfully!");
        }

        public void ProcessObjects()
        {
            _logger.Log($"Start processing job objects...");
            _backupJoba.ProcessObjects();
            _logger.Log($"Job objects processed successfully!");
        }

        public void RemoveJobObject(IJobObject jobObject)
        {
            _logger.Log($"Start removing job object with name {jobObject.Name}...");
            _backupJoba.RemoveJobObject(jobObject);
            _logger.Log($"Job object with name {jobObject.Name} removed successfully!");
        }

        public void UpdateSavedRestorePointsInfo(List<RestorePointFileDirectoryInfo> restorePointFileDirectoryInfos, List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos)
        {
            _logger.Log("Start updating restore points info...");
            _backupJoba.UpdateSavedRestorePointsInfo(restorePointFileDirectoryInfos, restorePointJobObjectsInfos);
            _logger.Log("Restore points info updated successfully!");
        }
    }
}
