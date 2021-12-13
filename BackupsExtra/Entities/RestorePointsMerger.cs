using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RestorePointsMerger : IRestorePointsReducer
    {
        public void ReduceRestorePoints(IBackupJoba backupJoba, List<uint> restorePointIds, uint firstSuitableRestorePointId)
        {
            for (int i = 0; i < restorePointIds.Count - 1; ++i)
            {
                backupJoba.Repository.MergeRestorePoints(restorePointIds[i], restorePointIds[i + 1]);
                List<RestorePointJobObjectsInfo> restorePointJobObjectsInfo = backupJoba.CreatedRestorePoints;
                MergeRestorePointJobObjectsInfos(restorePointJobObjectsInfo, restorePointIds[i], restorePointIds[i + 1]);
            }

            backupJoba.Repository.MergeRestorePoints(restorePointIds[restorePointIds.Count - 1], firstSuitableRestorePointId);

            List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos = backupJoba.CreatedRestorePoints;
            MergeRestorePointJobObjectsInfos(restorePointJobObjectsInfos, restorePointIds[restorePointIds.Count - 1], firstSuitableRestorePointId);
        }

        private void MergeRestorePointJobObjectsInfos(List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos, uint restorePointId1, uint restorePointId2)
        {
            RestorePointJobObjectsInfo restorePointInfo1 = restorePointJobObjectsInfos.Find(info => info.Id == restorePointId1);
            RestorePointJobObjectsInfo restorePointInfo2 = restorePointJobObjectsInfos.Find(info => info.Id == restorePointId2);
            var resultInfoJobObjectsNames = new List<string>(restorePointInfo2.FullJobObjectsNames);
            foreach (string jobObjectName in restorePointInfo1.FullJobObjectsNames)
            {
                if (!resultInfoJobObjectsNames.Contains(jobObjectName))
                {
                    resultInfoJobObjectsNames.Add(jobObjectName);
                }
            }

            var resultRestorePointInfo = new RestorePointJobObjectsInfo(restorePointInfo2.Id, restorePointInfo2.DateOfCreation, resultInfoJobObjectsNames);
            restorePointJobObjectsInfos.Remove(restorePointInfo1);
            restorePointJobObjectsInfos.Remove(restorePointInfo2);
            restorePointJobObjectsInfos.Add(resultRestorePointInfo);
        }
    }
}
