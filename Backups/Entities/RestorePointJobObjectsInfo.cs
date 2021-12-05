using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePointJobObjectsInfo
    {
        private List<string> _fullJobObjectNames;

        public RestorePointJobObjectsInfo(uint id, DateTime dateOfCreation, List<IJobObject> jobObjects)
        {
            Id = id;
            DateOfCreation = dateOfCreation;
            _fullJobObjectNames = new List<string>();
            foreach (IJobObject jobObject in jobObjects)
            {
                _fullJobObjectNames.Add(jobObject.Name + jobObject.Extension);
            }
        }

        public RestorePointJobObjectsInfo(uint id, DateTime dateOfCreation, List<string> fullJobObjectsNames)
        {
            Id = id;
            DateOfCreation = dateOfCreation;
            _fullJobObjectNames = fullJobObjectsNames;
        }

        public uint Id { get; }
        public DateTime DateOfCreation { get; }
        public IReadOnlyList<string> FullJobObjectsNames => _fullJobObjectNames;
    }
}
