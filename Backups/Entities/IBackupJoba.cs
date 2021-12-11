using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IBackupJoba
    {
        string Name { get; }
        IReadOnlyList<IJobObject> CurrentObjects { get; }
        IStorageAlgorithm StorageAlgorithm { get; }
        IRepository Repository { get; }
        List<RestorePointJobObjectsInfo> CreatedRestorePoints { get; }

        void AddJobObject(IJobObject jobObject);

        void RemoveJobObject(IJobObject jobObject);

        void ProcessObjects();

        void UpdateSavedRestorePointsInfo(List<RestorePointFileDirectoryInfo> restorePointFileDirectoryInfos, List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos);
    }
}
