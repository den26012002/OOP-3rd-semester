using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IBackupJobaConfigurationRepresenter
    {
        IBackupJoba RepresentedBackupJoba { get; }
        byte[] GetConfigurationRepresentation();
        byte[] GetSavedRestorePointsRepresentation();
    }
}
