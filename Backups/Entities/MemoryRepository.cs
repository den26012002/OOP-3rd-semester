using System.Collections.Generic;

namespace Backups.Entities
{
    public class MemoryRepository : IRepository
    {
        private List<MemoryRestorePointDirectory> _memoryRestorePointDirectories;

        public MemoryRepository()
        {
            _memoryRestorePointDirectories = new List<MemoryRestorePointDirectory>();
        }

        public void SaveRestorePoint(RestorePoint restorePoint)
        {
            _memoryRestorePointDirectories.Add(new MemoryRestorePointDirectory(restorePoint));
        }
    }
}
