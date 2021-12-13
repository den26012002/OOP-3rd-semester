using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IRestorePointsRestorer
    {
        void RestoreJobObjectsToOriginalLocation(List<IJobObject> jobObjects, RestorePointJobObjectsInfo restorePointJobObjectsInfo);

        void RestoreJobObjectsToDifferentLocation(List<IJobObject> jobObjects, string location);
    }
}
