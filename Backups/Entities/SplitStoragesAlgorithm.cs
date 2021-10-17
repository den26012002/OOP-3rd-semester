using System.Collections.Generic;

namespace Backups.Entities
{
    public class SplitStoragesAlgorithm : IStorageAlgorithm
    {
        public List<Storage> OrganizeJobObjects(List<IJobObject> jobObjects)
        {
            var jobObjectsGroups = new List<Storage>();
            foreach (IJobObject jobObject in jobObjects)
            {
                var jobObjectsList = new List<IJobObject>();
                jobObjectsList.Add(jobObject);
                var jobObjectsGroup = new Storage(jobObjectsList);
                jobObjectsGroups.Add(jobObjectsGroup);
            }

            return jobObjectsGroups;
        }
    }
}
