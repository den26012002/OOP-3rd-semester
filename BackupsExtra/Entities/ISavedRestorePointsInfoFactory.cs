using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface ISavedRestorePointsInfoFactory
    {
        List<RestorePointFileDirectoryInfo> GetRestorePointFileDirectoryInfos();

        List<RestorePointJobObjectsInfo> GetRestorePointJobObjectsInfos();
    }
}
