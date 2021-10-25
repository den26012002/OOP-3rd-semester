using System.Collections.Generic;

namespace Backups.Entities
{
    public class MemoryRestorePointDirectory
    {
        private List<MemoryStorage> _memoryStorages;

        public MemoryRestorePointDirectory(RestorePoint restorePoint)
        {
            _memoryStorages = new List<MemoryStorage>();
            foreach (Storage storage in restorePoint.Storages)
            {
                _memoryStorages.Add(new MemoryStorage(storage));
            }
        }

        public IReadOnlyList<MemoryStorage> MemoryStorages => _memoryStorages;
    }
}
