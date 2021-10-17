using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePointJobObjectsInfo
    {
        private List<string> _fullJobObjectNames;

        internal RestorePointJobObjectsInfo(List<IJobObject> jobObjects)
        {
            _fullJobObjectNames = new List<string>();
            foreach (IJobObject jobObject in jobObjects)
            {
                _fullJobObjectNames.Add(jobObject.Name + jobObject.Extension);
            }
        }

        public IReadOnlyList<string> FullJobObjectsNames => _fullJobObjectNames;
    }
}
