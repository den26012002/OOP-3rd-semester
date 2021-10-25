namespace Backups.Entities
{
    public interface IRepository
    {
        void SaveRestorePoint(RestorePoint restorePoint);
    }
}
