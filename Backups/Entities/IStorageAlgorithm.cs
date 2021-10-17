using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IStorageAlgorithm
    {
        List<Storage> OrganizeJobObjects(List<IJobObject> jobObjects);
    }
}
