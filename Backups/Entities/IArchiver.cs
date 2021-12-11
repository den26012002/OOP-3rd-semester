using System.Collections.Generic;

namespace Backups.Entities
{
    public interface IArchiver
    {
        void SaveStorage(Storage storage, string storageDirectoryPath, string storageName);

        List<Storage> LoadStorages(string storageDirectoryPath);
    }
}
