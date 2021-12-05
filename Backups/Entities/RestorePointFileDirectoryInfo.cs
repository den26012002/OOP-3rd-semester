namespace Backups.Entities
{
    public class RestorePointFileDirectoryInfo
    {
        public RestorePointFileDirectoryInfo(uint restorePointId, string restorePointDirectory)
        {
            RestorePointId = restorePointId;
            RestorePointDirectory = restorePointDirectory;
        }

        public uint RestorePointId { get; }
        public string RestorePointDirectory { get; }
    }
}
