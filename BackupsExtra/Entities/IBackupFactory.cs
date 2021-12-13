using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IBackupFactory
    {
        IBackupJoba GetBackupJoba();

        IStorageAlgorithm GetStorageAlgorithm();

        IRepository GetRepository();

        IArchiver GetArchiver();

        IStorageNameGiver GetStorageNameGiver();

        ILogger GetLogger();

        IRestorePointsDeletingAlgorithm GetRestorePointsDeletingAlgorithm();

        IRestorePointsReducer GetRestorePointsReducer();
    }
}
