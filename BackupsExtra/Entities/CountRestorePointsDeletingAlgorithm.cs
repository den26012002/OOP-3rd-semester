using System;
using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class CountRestorePointsDeletingAlgorithm : IRestorePointsDeletingAlgorithm
    {
        private uint _maxRestorePoints;

        public CountRestorePointsDeletingAlgorithm(uint maxRestorePoints)
        {
            _maxRestorePoints = maxRestorePoints;
        }

        public uint MaxRestorePoints => _maxRestorePoints;

        public List<uint> SelectUnsuitableRestorePointsIds(List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos, out uint firstSuitableRestorePointId)
        {
            var restorePointJobObjectsInfosCopy = new List<RestorePointJobObjectsInfo>(restorePointJobObjectsInfos);
            restorePointJobObjectsInfosCopy.Sort(new RestorePointsJobObjectsInfoComparer());
            var selectedRestorePointJobObjectsInfosIds = new List<uint>();
            for (int i = 0; i < restorePointJobObjectsInfosCopy.Count - (int)_maxRestorePoints; ++i)
            {
                selectedRestorePointJobObjectsInfosIds.Add(restorePointJobObjectsInfosCopy[i].Id);
            }

            firstSuitableRestorePointId = restorePointJobObjectsInfosCopy[Math.Max(restorePointJobObjectsInfosCopy.Count - (int)_maxRestorePoints, 0)].Id;
            return selectedRestorePointJobObjectsInfosIds;
        }
    }
}
