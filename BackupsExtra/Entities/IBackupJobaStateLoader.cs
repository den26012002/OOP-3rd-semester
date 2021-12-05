using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IBackupJobaStateLoader
    {
        IBackupJoba LoadState();
    }
}
