using System.Collections.Generic;

namespace Backups.Entities
{
    public class Storage
    {
        private List<IJobObject> _jobObjects;

        public Storage(List<IJobObject> jobObjects)
        {
            _jobObjects = jobObjects;
        }

        public IReadOnlyList<IJobObject> JobObjects => _jobObjects;
    }
}
