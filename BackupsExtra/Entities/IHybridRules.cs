using System.Collections.Generic;

namespace BackupsExtra.Entities
{
    public interface IHybridRules
    {
        List<uint> GetRuleResult(
            List<uint> restorePointsIds1,
            uint firstSuitableRestorePointId1,
            List<uint> restorePointsIds2,
            uint firstSuitableRestorePointId2,
            out uint resultFirstSuitableRestorePoint);
    }
}
