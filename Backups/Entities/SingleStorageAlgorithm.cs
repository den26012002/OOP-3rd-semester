using System.Collections.Generic;

namespace Backups.Entities
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public List<Storage> OrganizeJobObjects(List<IJobObject> jobObjects)
        {
            var jobObjectsGroups = new List<Storage>();
            var jobObjectsList = new List<IJobObject>();
            foreach (IJobObject jobObject in jobObjects)
            {
                jobObjectsList.Add(jobObject);
            }

            var jobObjectsGroup = new Storage(jobObjectsList);
            jobObjectsGroups.Add(jobObjectsGroup);
            return jobObjectsGroups;
        }
    }
}
