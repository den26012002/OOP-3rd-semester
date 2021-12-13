using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobaDeletingRestoringDecorator : IBackupJoba
    {
        private IBackupJoba _backupJoba;
        private IRestorePointsDeletingAlgorithm _restorePointsDeletingAlgorithm;
        private IRestorePointsReducer _restorePointsReducer;
        private IRestorePointsRestorer _restorePointsRestorer;
        public BackupJobaDeletingRestoringDecorator(
            IBackupJoba backupJoba,
            IRestorePointsDeletingAlgorithm restorePointsDeletingAlgorithm,
            IRestorePointsReducer restorePointsReducer,
            IRestorePointsRestorer restorePointsRestorer)
        {
            _backupJoba = backupJoba;
            _restorePointsDeletingAlgorithm = restorePointsDeletingAlgorithm;
            _restorePointsReducer = restorePointsReducer;
            _restorePointsRestorer = restorePointsRestorer;
        }

        public string Name => _backupJoba.Name;

        public IReadOnlyList<IJobObject> CurrentObjects => _backupJoba.CurrentObjects;

        public IStorageAlgorithm StorageAlgorithm => _backupJoba.StorageAlgorithm;

        public IRepository Repository => _backupJoba.Repository;

        public List<RestorePointJobObjectsInfo> CreatedRestorePoints => _backupJoba.CreatedRestorePoints;

        public IBackupJoba OriginalBackupJoba => _backupJoba;
        public IRestorePointsDeletingAlgorithm RestorePointsDeletingAlgorithm => _restorePointsDeletingAlgorithm;
        public IRestorePointsReducer RestorePointsReducer => _restorePointsReducer;
        public IRestorePointsRestorer RestorePointsRestorer => _restorePointsRestorer;

        public void AddJobObject(IJobObject jobObject)
        {
            _backupJoba.AddJobObject(jobObject);
        }

        public void ProcessObjects()
        {
            _backupJoba.ProcessObjects();
            uint firstSuitableRestorePointId;
            List<uint> unsuitableRestorePointIds = _restorePointsDeletingAlgorithm.SelectUnsuitableRestorePointsIds(CreatedRestorePoints, out firstSuitableRestorePointId);
            _restorePointsReducer.ReduceRestorePoints(this, unsuitableRestorePointIds, firstSuitableRestorePointId);
        }

        public void RemoveJobObject(IJobObject jobObject)
        {
            _backupJoba.RemoveJobObject(jobObject);
        }

        public void UpdateSavedRestorePointsInfo(List<RestorePointFileDirectoryInfo> restorePointFileDirectoryInfos, List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos)
        {
            _backupJoba.UpdateSavedRestorePointsInfo(restorePointFileDirectoryInfos, restorePointJobObjectsInfos);
        }

        public void RestoreRestorePoint(uint restorePointId, string location = null)
        {
            List<IJobObject> restoredJobObjects = Repository.GetJobObjects(restorePointId);
            if (location == null)
            {
                _restorePointsRestorer.RestoreJobObjectsToOriginalLocation(restoredJobObjects, CreatedRestorePoints.Find(info => info.Id == restorePointId));
            }
            else
            {
                _restorePointsRestorer.RestoreJobObjectsToDifferentLocation(restoredJobObjects, location);
            }
        }
    }
}
