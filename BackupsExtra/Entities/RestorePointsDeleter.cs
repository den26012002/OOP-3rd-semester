using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RestorePointsDeleter : IRestorePointsReducer
    {
        public void ReduceRestorePoints(IBackupJoba backupJoba, List<uint> restorePointIds, uint firstSuitableRestorePointId)
        {
            foreach (uint restorePointId in restorePointIds)
            {
                backupJoba.Repository.RemoveRestorePoint(restorePointId);
                List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos = backupJoba.CreatedRestorePoints;
                restorePointJobObjectsInfos.Remove(restorePointJobObjectsInfos.Find(restorePointInfo => restorePointInfo.Id == restorePointId));
            }
        }
    }
}
