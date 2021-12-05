using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backups.Entities
{
    public class MemoryRepository : IRepository
    {
        private List<MemoryRestorePointDirectory> _memoryRestorePointDirectories;

        public MemoryRepository()
        {
            _memoryRestorePointDirectories = new List<MemoryRestorePointDirectory>();
        }

        public IReadOnlyList<MemoryRestorePointDirectory> MemoryRestorePointDirectories => _memoryRestorePointDirectories;

        public void SaveRestorePoint(RestorePoint restorePoint)
        {
            _memoryRestorePointDirectories.Add(new MemoryRestorePointDirectory(restorePoint));
        }

        public void RemoveRestorePoint(uint restorePointId)
        {
            _memoryRestorePointDirectories.Remove(_memoryRestorePointDirectories.Find(directory => directory.RestorePointId == restorePointId));
        }

        public void MergeRestorePoints(uint oldRestorePointId, uint newRestorePointId)
        {
            IReadOnlyList<MemoryStorage> oldDirectory = _memoryRestorePointDirectories.Find(directory => directory.RestorePointId == oldRestorePointId).MemoryStorages;
            IReadOnlyList<MemoryStorage> newDirectory = _memoryRestorePointDirectories.Find(directory => directory.RestorePointId == newRestorePointId).MemoryStorages;
            var resultDirectory = new List<MemoryStorage>();
            foreach (MemoryStorage memoryStorage in oldDirectory)
            {
                if (!newDirectory.Contains(memoryStorage))
                {
                    resultDirectory.Add(memoryStorage);
                }
            }

            MemoryRestorePointDirectory newRestorePoint = _memoryRestorePointDirectories.Find(directory => directory.RestorePointId == newRestorePointId);
            RemoveRestorePoint(oldRestorePointId);
            RemoveRestorePoint(newRestorePointId);
            _memoryRestorePointDirectories.Add(new MemoryRestorePointDirectory(newRestorePointId, resultDirectory));
        }

        public List<IJobObject> GetJobObjects(uint restorePointId)
        {
            MemoryRestorePointDirectory memoryRestorePointDirectory =
                _memoryRestorePointDirectories.Find(directory => directory.RestorePointId == restorePointId);
            var jobObjects = new List<IJobObject>();
            foreach (MemoryStorage memoryStorage in memoryRestorePointDirectory.MemoryStorages)
            {
                foreach (MemoryJobObject memoryJobObject in memoryStorage.JobObjects)
                {
                    jobObjects.Add(new UniversalJobObject(
                        memoryJobObject.FullName,
                        string.Empty,
                        Encoding.Default.GetString(memoryJobObject.Representation)));
                }
            }

            return jobObjects;
        }
    }
}
