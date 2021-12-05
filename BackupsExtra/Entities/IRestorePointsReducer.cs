using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IRestorePointsReducer
    {
        void ReduceRestorePoints(IBackupJoba backupJoba, List<uint> restorePointIds, uint firstSuitableRestorePointId);
    }
}
