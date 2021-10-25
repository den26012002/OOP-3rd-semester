using System.Collections.Generic;

namespace Backups.Entities
{
    public class MemoryStorage
    {
        private List<MemoryJobObject> _jobObjects;

        public MemoryStorage(Storage storage)
        {
            _jobObjects = new List<MemoryJobObject>();
            foreach (IJobObject jobObject in storage.JobObjects)
            {
                _jobObjects.Add(new MemoryJobObject(jobObject.Name + jobObject.Extension, jobObject.GetRepresentation()));
            }
        }

        public List<MemoryJobObject> JobObjects => _jobObjects;
    }
}
