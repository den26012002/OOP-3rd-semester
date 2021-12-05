using System;
using System.Collections.Generic;

namespace BackupsExtra.Entities
{
    public class AndHybridRules : IHybridRules
    {
        public List<uint> GetRuleResult(
            List<uint> restorePointsIds1,
            uint firstSuitableRestorePointId1,
            List<uint> restorePointsIds2,
            uint firstSuitableRestorePointId2,
            out uint resultFirstSuitableRestorePoint)
        {
            var resultRestorePointsIds = new List<uint>();
            foreach (uint restorePointId1 in restorePointsIds1)
            {
                if (restorePointsIds2.Contains(restorePointId1))
                {
                    resultRestorePointsIds.Add(restorePointId1);
                }
            }

            resultFirstSuitableRestorePoint = Math.Min(firstSuitableRestorePointId1, firstSuitableRestorePointId2);
            return resultRestorePointsIds;
        }
    }
}
