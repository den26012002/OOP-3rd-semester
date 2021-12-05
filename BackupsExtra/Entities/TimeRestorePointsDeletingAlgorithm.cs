using System;
using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class TimeRestorePointsDeletingAlgorithm : IRestorePointsDeletingAlgorithm
    {
        private DateTime _oldestRestorePointTime;

        public TimeRestorePointsDeletingAlgorithm(DateTime oldestRestorePointTime)
        {
            _oldestRestorePointTime = oldestRestorePointTime;
        }

        public DateTime OldestRestorePointTime => _oldestRestorePointTime;

        public List<uint> SelectUnsuitableRestorePointsIds(List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos, out uint firstSuitableRestorePointId)
        {
            var selectedInfosIds = new List<uint>();
            firstSuitableRestorePointId = uint.MaxValue;
            foreach (RestorePointJobObjectsInfo restorePointJobObjectsInfo in restorePointJobObjectsInfos)
            {
                if (restorePointJobObjectsInfo.DateOfCreation <= _oldestRestorePointTime)
                {
                    selectedInfosIds.Add(restorePointJobObjectsInfo.Id);
                }
                else
                {
                    if (restorePointJobObjectsInfo.Id < firstSuitableRestorePointId)
                    {
                        firstSuitableRestorePointId = restorePointJobObjectsInfo.Id;
                    }
                }
            }

            return selectedInfosIds;
        }
    }
}
