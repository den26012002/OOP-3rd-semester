using System;
using System.Collections.Generic;
using System.Text.Json;
using Backups.Entities;
using Backups.Tools;

namespace BackupsExtra.Entities
{
    public class BackupsJsonFactory : IBackupFactory
    {
        private JsonElement _backupJobaJsonElement;
        public BackupsJsonFactory(JsonElement backupJobaJsonElement)
        {
            _backupJobaJsonElement = backupJobaJsonElement;
        }

        public IBackupJoba GetBackupJoba()
        {
            return GetBackupJoba(_backupJobaJsonElement);
        }

        public IStorageAlgorithm GetStorageAlgorithm()
        {
            string algorithmName = _backupJobaJsonElement.GetProperty("StorageAlgorithm").GetProperty("Type").GetType().Name;
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

        public IRepository GetRepository()
        {
            return GetRepository(_backupJobaJsonElement.GetProperty("Repository"));
        }

        public IArchiver GetArchiver()
        {
            string archiverName = _backupJobaJsonElement.GetProperty("Archiver").GetProperty("Type").ToString();
            switch (archiverName)
            {
                case "ZipArchiver":
                    return new ZipArchiver();
                default:
                    throw new BackupsException("Error: unknown archiver");
            }
        }

        public IStorageNameGiver GetStorageNameGiver()
        {
            string storageNameGiverName = _backupJobaJsonElement.GetProperty("StorageNameGiver").GetProperty("Type").ToString();
            switch (storageNameGiverName)
            {
                case "DefaultStorageNameGiver":
                    return new DefaultStorageNameGiver();
                default:
                    throw new BackupsException("Error: unknown storage name giver");
            }
        }

        public ILogger GetLogger()
        {
            return GetLogger(_backupJobaJsonElement.GetProperty("Logger"));
        }

        public IRestorePointsDeletingAlgorithm GetRestorePointsDeletingAlgorithm()
        {
            return GetRestorePointsDeletingAlgorithm(_backupJobaJsonElement.GetProperty("RestorePointsDeletingAlgorithm"));
        }

        public IRestorePointsReducer GetRestorePointsReducer()
        {
            return GetRestorePointsReducer(_backupJobaJsonElement.GetProperty("RestorePointsReducer"));
        }

        private ILogger GetLogger(JsonElement loggerJsonElement)
        {
            string loggerType = loggerJsonElement.GetProperty("Type").ToString();
            switch (loggerType)
            {
                case "ConsoleLogger":
                    return new ConsoleLogger();
                case "FileLogger":
                    return new FileLogger(loggerJsonElement.GetProperty("LogFileName").ToString());
                case "LoggerTimeDecorator":
                    return new LoggerTimeDecorator(GetLogger(loggerJsonElement.GetProperty("OriginalLogger")));
                default:
                    throw new BackupsException("Error: unknown logger");
            }
        }

        private IRestorePointsDeletingAlgorithm GetRestorePointsDeletingAlgorithm(JsonElement deletingAlgorithmJsonElement)
        {
            switch (deletingAlgorithmJsonElement.GetProperty("Type").ToString())
            {
                case "CountRestorePointsDeletingAlgorithm":
                    return new CountRestorePointsDeletingAlgorithm(
                         uint.Parse(deletingAlgorithmJsonElement.GetProperty("MaxRestorePoints").ToString()));
                case "TimeRestorePointsDeletingAlgorithm":
                    return new TimeRestorePointsDeletingAlgorithm(
                        DateTime.Parse(deletingAlgorithmJsonElement.GetProperty("OldestRestorePointTime").ToString()));
                case "HybridRestorePointsDeletingAlgorithm":
                    deletingAlgorithmJsonElement.EnumerateArray().MoveNext();
                    JsonElement restorePointsDeletingAlgorithmsJsonElement =
                        deletingAlgorithmJsonElement.GetProperty("RestorePointsDeletingAlgorithms");
                    JsonElement.ArrayEnumerator deletingAlgorithmsEnumerator = restorePointsDeletingAlgorithmsJsonElement.EnumerateArray();
                    int arrayLength = restorePointsDeletingAlgorithmsJsonElement.GetArrayLength();
                    var deletingAlgorithms = new List<IRestorePointsDeletingAlgorithm>();
                    for (int i = 0; i < arrayLength; ++i)
                    {
                        deletingAlgorithms.Add(GetRestorePointsDeletingAlgorithm(deletingAlgorithmsEnumerator.Current));
                        deletingAlgorithmsEnumerator.MoveNext();
                    }

                    return new HybridRestorePointsDeletingAlgorithm(
                        deletingAlgorithms,
                        GetHybridRules(deletingAlgorithmJsonElement.GetProperty("HybridRules")));
                default:
                    throw new BackupsException("Error: unknown restore points deleting algorithm");
            }
        }

        private IHybridRules GetHybridRules(JsonElement hybridRulesJsonElement)
        {
            switch (hybridRulesJsonElement.GetProperty("Type").ToString())
            {
                case "OrHybridRules":
                    return new OrHybridRules();
                case "AndHybridRules":
                    return new AndHybridRules();
                default:
                    throw new BackupsException("Error: unknown hybrid rules");
            }
        }

        private IRepository GetRepository(JsonElement repositoryJsonElement)
        {
            switch (repositoryJsonElement.GetProperty("Type").ToString())
            {
                case "RepositoryLogDecorator":
                    return new RepositoryLogDecorator(
                        GetRepository(repositoryJsonElement.GetProperty("OriginalBackupJoba")),
                        GetLogger(repositoryJsonElement.GetProperty("Logger")));
                case "FileSystemRepository":
                    return new FileSystemRepository(
                        repositoryJsonElement.GetProperty("BackupsDirectoryPath").ToString(),
                        GetArchiver(),
                        GetStorageNameGiver());
                case "MemoryRepository":
                    return new MemoryRepository();
                default:
                    throw new BackupsException("Error: unknown repository");
            }
        }

        private IRestorePointsReducer GetRestorePointsReducer(JsonElement restorePointsReducerJsonElement)
        {
            switch (restorePointsReducerJsonElement.GetProperty("Type").ToString())
            {
                case "RestorePointsDeleter":
                    return new RestorePointsDeleter();
                case "RestorePointsMerger":
                    return new RestorePointsMerger();
                default:
                    throw new BackupsException("Error: unknown restore points reducer");
            }
        }

        private IRestorePointsRestorer GetRestorePointsRestorer(JsonElement restorePointsRestorerJsonElement)
        {
            switch (restorePointsRestorerJsonElement.GetProperty("Type").ToString())
            {
                case "FileSystemRestorePointsRestorer":
                    return new FileSystemRestorePointsRestorer();
                default:
                    throw new BackupsException("Error: unknown restore points restorer");
            }
        }

        private IBackupJoba GetBackupJoba(JsonElement backupJobaJsonElement)
        {
            switch (backupJobaJsonElement.GetProperty("Type").ToString())
            {
                case "BackupJobaLogDecorator":
                    return new BackupJobaLogDecorator(
                        GetBackupJoba(backupJobaJsonElement.GetProperty("OriginalBackupJoba")),
                        GetLogger(backupJobaJsonElement.GetProperty("Logger")));
                case "BackupJobaDeletingRestoringDecorator":
                    return new BackupJobaDeletingRestoringDecorator(
                        GetBackupJoba(backupJobaJsonElement.GetProperty("OriginalBackupJoba")),
                        GetRestorePointsDeletingAlgorithm(backupJobaJsonElement.GetProperty("RestorePointsDeletingAlgorithm")),
                        GetRestorePointsReducer(backupJobaJsonElement.GetProperty("RestorePointsReducer")),
                        GetRestorePointsRestorer(backupJobaJsonElement.GetProperty("RestorePointsRestorer")));
                case "BackupJoba":
                    return new BackupJoba(
                        backupJobaJsonElement.GetProperty("Name").ToString(),
                        GetStorageAlgorithm(),
                        GetRepository());
                default:
                    throw new BackupsException("Error: unknown backup joba");
            }
        }
    }
}
