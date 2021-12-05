using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IBackupJoba
    {
        public string Name { get; }
        public IReadOnlyList<IJobObject> CurrentObjects { get; }
        public IStorageAlgorithm StorageAlgorithm { get; }
        public IRepository Repository { get; }
        public List<RestorePointJobObjectsInfo> CreatedRestorePoints { get; }

        public void AddJobObject(IJobObject jobObject);

        public void RemoveJobObject(IJobObject jobObject);

        public void ProcessObjects();

        public void UpdateSavedRestorePointsInfo(List<RestorePointFileDirectoryInfo> restorePointFileDirectoryInfos, List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos);
    }
}
