using Backups.Entities;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        [Test]
        public void SplitAdd2FilesCreateRestorePointRemoveFileCreateRestorePoint_2RestorePointsAnd3StoragesCreated()
        {
            var splitStorageAlgorithm = new SplitStoragesAlgorithm();
            var memoryRepository = new MemoryRepository();
            var backupJoba = new BackupJoba("Backup", splitStorageAlgorithm, memoryRepository);

            var file1 = new EmptyJobObject();
            var file2 = new EmptyJobObject();
            backupJoba.AddJobObject(file1);
            backupJoba.AddJobObject(file2);
            backupJoba.ProcessObjects();
            backupJoba.RemoveJobObject(file1);
            backupJoba.ProcessObjects();

            Assert.AreEqual(memoryRepository.MemoryRestorePointDirectories.Count, 2);
            Assert.AreEqual(memoryRepository.MemoryRestorePointDirectories[0].MemoryStorages.Count +
                memoryRepository.MemoryRestorePointDirectories[1].MemoryStorages.Count, 3);
        }
    }
}
