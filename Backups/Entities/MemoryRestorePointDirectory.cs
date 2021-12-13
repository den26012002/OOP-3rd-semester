using System.Collections.Generic;

namespace Backups.Entities
{
    public class MemoryRestorePointDirectory
    {
        private List<MemoryStorage> _memoryStorages;

        public MemoryRestorePointDirectory(RestorePoint restorePoint)
        {
            RestorePointId = restorePoint.Id;
            _memoryStorages = new List<MemoryStorage>();
            foreach (Storage storage in restorePoint.Storages)
            {
                _memoryStorages.Add(new MemoryStorage(storage));
            }
        }

        internal MemoryRestorePointDirectory(uint restorePointId, List<MemoryStorage> memoryStorages)
        {
            RestorePointId = restorePointId;
            _memoryStorages = memoryStorages;
        }

        public uint RestorePointId { get; }
        public IReadOnlyList<MemoryStorage> MemoryStorages => _memoryStorages;
    }
}
