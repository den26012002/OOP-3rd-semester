using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backups.Entities;
using Backups.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJobaJsonRepresenter : IBackupJobaConfigurationRepresenter
    {
        public BackupJobaJsonRepresenter(IBackupJoba backupJoba)
        {
            RepresentedBackupJoba = backupJoba;
        }

        public IBackupJoba RepresentedBackupJoba { get; }

        public byte[] GetConfigurationRepresentation()
        {
            return Encoding.Default.GetBytes(GetBackupJobaRepresentation(RepresentedBackupJoba));
        }

        public byte[] GetSavedRestorePointsRepresentation()
        {
            var savedRestorePointsRepresentationBuilder = new JsonStringBuilder();
            IBackupJoba backupJoba = RepresentedBackupJoba;
            while (backupJoba is BackupJobaDeletingRestoringDecorator backupJobaDecorator)
            {
                backupJoba = backupJobaDecorator.OriginalBackupJoba;
            }

            IRepository backupJobaRepository = backupJoba.Repository;
            while (backupJobaRepository is RepositoryLogDecorator repositoryLogDecorator)
            {
                backupJobaRepository = repositoryLogDecorator.OriginalRepository;
            }

            if (backupJobaRepository is FileSystemRepository fileSystemRepository)
            {
                var savedRestorePoints = new List<string>();
                foreach (RestorePointFileDirectoryInfo restorePointFileDirectoryInfo in fileSystemRepository.RestorePointFileDirectoryInfos)
                {
                    var savedRestorePointsBuilder = new JsonStringBuilder();
                    savedRestorePointsBuilder.AddProperty("RestorePointId", restorePointFileDirectoryInfo.RestorePointId.ToString());
                    savedRestorePointsBuilder.AddProperty("RestorePointDirectory", restorePointFileDirectoryInfo.RestorePointDirectory);
                    savedRestorePoints.Add(savedRestorePointsBuilder.GetResult());
                }

                savedRestorePointsRepresentationBuilder.AddPropertyObject("SavedRestorePoints", savedRestorePoints.ToArray());

                var savedRestorePointsJobObjects = new List<string>();
                foreach (RestorePointJobObjectsInfo restorePointJobObjectsInfo in backupJoba.CreatedRestorePoints)
                {
                    var savedRestorePointsJobObjectsInfosBuilder = new JsonStringBuilder();
                    savedRestorePointsJobObjectsInfosBuilder.AddProperty("Id", restorePointJobObjectsInfo.Id.ToString());
                    savedRestorePointsJobObjectsInfosBuilder.AddProperty("DateOfCreation", restorePointJobObjectsInfo.DateOfCreation.ToString());
                    savedRestorePointsJobObjectsInfosBuilder.AddProperty("FullJobObjectsNames", restorePointJobObjectsInfo.FullJobObjectsNames.ToArray());
                    savedRestorePointsJobObjects.Add(savedRestorePointsJobObjectsInfosBuilder.GetResult());
                }

                savedRestorePointsRepresentationBuilder.AddPropertyObject("RestorePointJobObjectsInfos", savedRestorePointsJobObjects.ToArray());
            }

            return Encoding.Default.GetBytes(savedRestorePointsRepresentationBuilder.GetResult());
        }

        private string GetBackupJobaRepresentation(IBackupJoba backupJoba)
        {
            var backupJobaRepresentationBuilder = new JsonStringBuilder();
            backupJobaRepresentationBuilder.AddProperty("Type", backupJoba.GetType().Name);
            if (backupJoba is BackupJobaLogDecorator backupJobaLogDecorator)
            {
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "OriginalBackupJoba",
                    GetBackupJobaRepresentation(backupJobaLogDecorator.OriginalBackupJoba));
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "Logger",
                    GetLoggerRepresentation(backupJobaLogDecorator.Logger));
            }
            else if (backupJoba is BackupJobaDeletingRestoringDecorator backupJobaDecorator)
            {
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "OriginalBackupJoba",
                    GetBackupJobaRepresentation(backupJobaDecorator.OriginalBackupJoba));
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "RestorePointsDeletingAlgorithm",
                    GetRestorePointsDeletingAlgorithmRepresentation(backupJobaDecorator.RestorePointsDeletingAlgorithm));
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "RestorePointsReducer",
                    GetRestorePointsReducerRepresentation(backupJobaDecorator.RestorePointsReducer));
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "RestorePointsRestorer",
                    GetRestorePointsRestorerRepresentation(backupJobaDecorator.RestorePointsRestorer));
            }
            else if (backupJoba is BackupJoba realBackupJoba)
            {
                backupJobaRepresentationBuilder.AddProperty("Name", realBackupJoba.Name);
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "StorageAlgorithm",
                    GetStorageAlgorithmRepresentation(realBackupJoba.StorageAlgorithm));
                backupJobaRepresentationBuilder.AddPropertyObject(
                    "Repository",
                    GetRepositoryRepresentation(realBackupJoba.Repository));
            }

            return backupJobaRepresentationBuilder.GetResult();
        }

        private string GetStorageAlgorithmRepresentation(IStorageAlgorithm storageAlgorithm)
        {
            var storageAlgorithmRepresentationBuilder = new JsonStringBuilder();
            storageAlgorithmRepresentationBuilder.AddProperty("Type", storageAlgorithm.GetType().Name);

            return storageAlgorithmRepresentationBuilder.GetResult();
        }

        private string GetRepositoryRepresentation(IRepository repository)
        {
            var repositoryRepresentationBuilder = new JsonStringBuilder();
            repositoryRepresentationBuilder.AddProperty("Type", repository.GetType().Name);
            if (repository is RepositoryLogDecorator repositoryLogDecorator)
            {
                repositoryRepresentationBuilder.AddPropertyObject("Logger", GetLoggerRepresentation(repositoryLogDecorator.Logger));
                repositoryRepresentationBuilder.AddPropertyObject(
                    $"OriginalRepository",
                    GetRepositoryRepresentation(repositoryLogDecorator.OriginalRepository));
            }
            else if (repository is FileSystemRepository fileSystemRepository)
            {
                repositoryRepresentationBuilder.AddPropertyObject(
                    "BackupsDirectoryPath",
                    fileSystemRepository.BackupsDirectoryPath);
                repositoryRepresentationBuilder.AddPropertyObject(
                    "Archiver",
                    GetArchiverRepresentation(fileSystemRepository.Archiver));
                repositoryRepresentationBuilder.AddPropertyObject(
                    "StorageNameGiver",
                    GetStorageNameGiverRepresentation(fileSystemRepository.StorageNameGiver));
                IReadOnlyList<RestorePointFileDirectoryInfo> directoryInfos = fileSystemRepository.RestorePointFileDirectoryInfos;
                var directoryInfosRepresentation = new List<string>();
                foreach (RestorePointFileDirectoryInfo directoryInfo in directoryInfos)
                {
                    directoryInfosRepresentation.Add(GetRestorePointFileDirectoryInfoRepresentation(directoryInfo));
                }

                repositoryRepresentationBuilder.AddPropertyObject(
                    "RestorePointFileDirectoryInfos",
                    directoryInfosRepresentation.ToArray());
            }

            return repositoryRepresentationBuilder.GetResult();
        }

        private string GetLoggerRepresentation(ILogger logger)
        {
            var loggerRepresentationBuilder = new JsonStringBuilder();
            loggerRepresentationBuilder.AddProperty("Type", $"{logger.GetType().Name}");
            if (logger is LoggerTimeDecorator loggerTimeDecorator)
            {
                loggerRepresentationBuilder.AddPropertyObject(
                    "OriginalLogger",
                    GetLoggerRepresentation(loggerTimeDecorator.OriginalLogger));
            }

            if (logger is FileLogger fileLogger)
            {
                loggerRepresentationBuilder.AddProperty("LogFileName", fileLogger.LogFileName);
            }

            return loggerRepresentationBuilder.GetResult();
        }

        private string GetArchiverRepresentation(IArchiver archiver)
        {
            var archiverRepresentationBuilder = new JsonStringBuilder();
            archiverRepresentationBuilder.AddProperty("Type", archiver.GetType().Name);

            return archiverRepresentationBuilder.GetResult();
        }

        private string GetRestorePointFileDirectoryInfoRepresentation(RestorePointFileDirectoryInfo restorePointFileDirectoryInfo)
        {
            var restorePointFileDirectoryInfoRepresentationBuilder = new JsonStringBuilder();
            restorePointFileDirectoryInfoRepresentationBuilder.AddProperty(
                "RestorePointId",
                restorePointFileDirectoryInfo.RestorePointId.ToString());
            restorePointFileDirectoryInfoRepresentationBuilder.AddProperty(
                "RestorePointDirectory",
                restorePointFileDirectoryInfo.RestorePointDirectory);

            return restorePointFileDirectoryInfoRepresentationBuilder.GetResult();
        }

        private string GetStorageNameGiverRepresentation(IStorageNameGiver storageNameGiver)
        {
            var storageNameGiverRepresentationBuilder = new JsonStringBuilder();
            storageNameGiverRepresentationBuilder.AddProperty("Type", storageNameGiver.GetType().Name);

            return storageNameGiverRepresentationBuilder.GetResult();
        }

        private string GetRestorePointsDeletingAlgorithmRepresentation(IRestorePointsDeletingAlgorithm restorePointsDeletingAlgorithm)
        {
            var restorePointsDeletingAlgorithmRepresentationBuilder = new JsonStringBuilder();
            restorePointsDeletingAlgorithmRepresentationBuilder.AddProperty("Type", restorePointsDeletingAlgorithm.GetType().Name);
            if (restorePointsDeletingAlgorithm is CountRestorePointsDeletingAlgorithm countRestorePointsDeletingAlgorithm)
            {
                restorePointsDeletingAlgorithmRepresentationBuilder.AddProperty(
                    "MaxRestorePoints",
                    countRestorePointsDeletingAlgorithm.MaxRestorePoints.ToString());
            }

            if (restorePointsDeletingAlgorithm is TimeRestorePointsDeletingAlgorithm timeRestorePointsDeletingAlgorithm)
            {
                restorePointsDeletingAlgorithmRepresentationBuilder.AddProperty(
                    "OldestRestorePointTime",
                    timeRestorePointsDeletingAlgorithm.OldestRestorePointTime.ToString());
            }

            if (restorePointsDeletingAlgorithm is HybridRestorePointsDeletingAlgorithm hybridRestorePointsDeletingAlgorithm)
            {
                var restorePointsDeletingAlgorithmsRepresentatoins = new List<string>();
                foreach (IRestorePointsDeletingAlgorithm pointsDeletingAlgorithm in hybridRestorePointsDeletingAlgorithm.RestorePointsDeletingAlgorithms)
                {
                    restorePointsDeletingAlgorithmsRepresentatoins.Add(GetRestorePointsDeletingAlgorithmRepresentation(pointsDeletingAlgorithm));
                }

                restorePointsDeletingAlgorithmRepresentationBuilder.AddPropertyObject(
                    "RestorePointsDeletingAlgorithms",
                    restorePointsDeletingAlgorithmsRepresentatoins.ToArray());
                restorePointsDeletingAlgorithmRepresentationBuilder.AddPropertyObject(
                    "HybridRules",
                    GetHybridRulesRepresentation(hybridRestorePointsDeletingAlgorithm.HybridRules));
            }

            return restorePointsDeletingAlgorithmRepresentationBuilder.GetResult();
        }

        private string GetHybridRulesRepresentation(IHybridRules hybridRules)
        {
            var hybridRulesRepresentationBuilder = new JsonStringBuilder();
            hybridRulesRepresentationBuilder.AddProperty("Type", hybridRules.GetType().Name);

            return hybridRulesRepresentationBuilder.GetResult();
        }

        private string GetRestorePointsReducerRepresentation(IRestorePointsReducer restorePointsReducer)
        {
            var restorePointsReducerRepresentationBuilder = new JsonStringBuilder();
            restorePointsReducerRepresentationBuilder.AddProperty("Type", restorePointsReducer.GetType().Name);

            return restorePointsReducerRepresentationBuilder.GetResult();
        }

        private string GetRestorePointsRestorerRepresentation(IRestorePointsRestorer restorePointsRestorer)
        {
            var restorePointsRestorerRepresentationBuilder = new JsonStringBuilder();
            restorePointsRestorerRepresentationBuilder.AddProperty("Type", restorePointsRestorer.GetType().Name);

            return restorePointsRestorerRepresentationBuilder.GetResult();
        }
    }
}
