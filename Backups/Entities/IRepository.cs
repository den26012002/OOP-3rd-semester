using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IRepository
    {
        void SaveRestorePoint(RestorePoint restorePoint);

        void RemoveRestorePoint(uint restorePointId);

        void MergeRestorePoints(uint oldRestorePointId, uint newRestorePointId);

        List<IJobObject> GetJobObjects(uint restorePointId);
    }
}
