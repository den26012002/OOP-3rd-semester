using System;
using System.Collections.Generic;

namespace BackupsExtra.Entities
{
    public class OrHybridRules : IHybridRules
    {
        public List<uint> GetRuleResult(
            List<uint> restorePointsIds1,
            uint firstSuitableRestorePointId1,
            List<uint> restorePointsIds2,
            uint firstSuitableRestorePointId2,
            out uint resultFirstSuitableRestorePoint)
        {
            var resultRestorePointsIds = new List<uint>(restorePointsIds1);
            foreach (uint restorePointId2 in restorePointsIds2)
            {
                if (!resultRestorePointsIds.Contains(restorePointId2))
                {
                    resultRestorePointsIds.Add(restorePointId2);
                }
            }

            resultFirstSuitableRestorePoint = Math.Max(firstSuitableRestorePointId1, firstSuitableRestorePointId2);
            return resultRestorePointsIds;
        }
    }
}
