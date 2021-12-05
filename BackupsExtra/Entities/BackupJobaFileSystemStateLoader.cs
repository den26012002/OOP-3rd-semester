using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobaFileSystemStateLoader : IBackupJobaStateLoader
    {
        private IBackupFactory _backupFactory;
        private ISavedRestorePointsInfoFactory _savedRestorePointsInfoFactory;

        public BackupJobaFileSystemStateLoader(
            IBackupFactory backupFactory,
            ISavedRestorePointsInfoFactory savedRestorePointsInfoFactory)
        {
            _backupFactory = backupFactory;
            _savedRestorePointsInfoFactory = savedRestorePointsInfoFactory;
        }

        public IBackupJoba LoadState()
        {
            IBackupJoba backupJoba = _backupFactory.GetBackupJoba();
            List<RestorePointFileDirectoryInfo> savedRestorePointFileDirectoryInfos = _savedRestorePointsInfoFactory.GetRestorePointFileDirectoryInfos();
            List<RestorePointJobObjectsInfo> savedRestorePointJobObjectsInfos = _savedRestorePointsInfoFactory.GetRestorePointJobObjectsInfos();
            backupJoba.UpdateSavedRestorePointsInfo(savedRestorePointFileDirectoryInfos, savedRestorePointJobObjectsInfos);

            return backupJoba;
        }
    }
}
