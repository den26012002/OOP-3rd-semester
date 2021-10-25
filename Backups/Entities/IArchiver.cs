namespace Backups.Entities
{
    public interface IArchiver
    {
        void SaveStorage(Storage storage, string storageDirectoryPath, string storageName);
    }
}
