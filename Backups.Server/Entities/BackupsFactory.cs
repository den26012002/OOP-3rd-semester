using System.Text.Json;
using Backups.Entities;
using Backups.Tools;

namespace Backups.Server.Entities
{
    public class BackupsFactory
    {
        public IStorageAlgorithm GetAlgorithm(string algorithmName)
        {
            switch (algorithmName)
            {
                case "SingleStorageAlgorithm":
                    return new SingleStorageAlgorithm();
                case "SplitStoragesAlgorithm":
                    return new SplitStoragesAlgorithm();
                default:
                    throw new BackupsException("Error: unknown algorithm");
            }
        }

        public IRepository GetRepository(JsonElement configurationJsonElement, string backupsDirectory = "")
        {
            string repositoryName = configurationJsonElement.GetProperty("Repository").ToString();
            switch (repositoryName)
            {
                case "FileSystemRepository":
                    string backupJobaName = configurationJsonElement.GetProperty("BackupJobaName").ToString();
                    string archiverName = configurationJsonElement.GetProperty("Archiver").ToString();
                    string storageNameGiverName = configurationJsonElement.GetProperty("StorageNameGiver").ToString();

                    IArchiver archiver = GetArchiver(archiverName);
                    IStorageNameGiver storageNameGiver = GetStorageNameGiver(storageNameGiverName);
                    return new FileSystemRepository(
                        backupsDirectory + "\\" + backupJobaName,
                        archiver,
                        storageNameGiver);
                case "MemoryRepository":
                    return new MemoryRepository();
                default:
                    throw new BackupsException("Error: unknown repository");
            }
        }

        public IArchiver GetArchiver(string archiverName)
        {
            switch (archiverName)
            {
                case "ZipArchiver":
                    return new ZipArchiver();
                default:
                    throw new BackupsException("Error: unknown archiver");
            }
        }

        public IStorageNameGiver GetStorageNameGiver(string storageNameGiverName)
        {
            switch (storageNameGiverName)
            {
                case "DefaultStorageNameGiver":
                    return new DefaultStorageNameGiver();
                default:
                    throw new BackupsException("Error: unknown storage name giver");
            }
        }
    }
}
