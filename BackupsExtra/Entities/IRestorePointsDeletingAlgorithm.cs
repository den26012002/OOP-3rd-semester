using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IRestorePointsDeletingAlgorithm
    {
        List<uint> SelectUnsuitableRestorePointsIds(List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos, out uint firstSuitableRestorePointId);
    }
}
