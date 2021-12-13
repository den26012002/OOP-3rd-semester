using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class HybridRestorePointsDeletingAlgorithm : IRestorePointsDeletingAlgorithm
    {
        private List<IRestorePointsDeletingAlgorithm> _restorePointsDeletingAlgorithms;
        private IHybridRules _hybridRules;

        public HybridRestorePointsDeletingAlgorithm(List<IRestorePointsDeletingAlgorithm> restorePointsDeletingAlgorithms, IHybridRules hybridRules)
        {
            _restorePointsDeletingAlgorithms = restorePointsDeletingAlgorithms;
            _hybridRules = hybridRules;
        }

        public IReadOnlyList<IRestorePointsDeletingAlgorithm> RestorePointsDeletingAlgorithms => _restorePointsDeletingAlgorithms;
        public IHybridRules HybridRules => _hybridRules;

        public List<uint> SelectUnsuitableRestorePointsIds(List<RestorePointJobObjectsInfo> restorePointJobObjectsInfos, out uint firstSuitableRestorePointId)
        {
            var selectedRestorePointIds = new List<uint>();
            firstSuitableRestorePointId = uint.MaxValue;
            foreach (IRestorePointsDeletingAlgorithm restorePointsDeletingAlgorithm in _restorePointsDeletingAlgorithms)
            {
                uint localFirstSuitableRestorePointId;
                List<uint> unsuitableRestorePointsIds = restorePointsDeletingAlgorithm.SelectUnsuitableRestorePointsIds(restorePointJobObjectsInfos, out localFirstSuitableRestorePointId);

                if (selectedRestorePointIds.Count == 0)
                {
                    selectedRestorePointIds = unsuitableRestorePointsIds;
                    firstSuitableRestorePointId = localFirstSuitableRestorePointId;
                }
                else
                {
                    selectedRestorePointIds = _hybridRules.GetRuleResult(
                        selectedRestorePointIds,
                        firstSuitableRestorePointId,
                        unsuitableRestorePointsIds,
                        localFirstSuitableRestorePointId,
                        out firstSuitableRestorePointId);
                }
            }

            return selectedRestorePointIds;
        }
    }
}
