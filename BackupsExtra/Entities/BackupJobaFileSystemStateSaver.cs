using System.IO;

namespace BackupsExtra.Entities
{
    public class BackupJobaFileSystemStateSaver : IBackupJobaStateSaver
    {
        private IBackupJobaConfigurationRepresenter _configurationRepresenter;
        private string _configurationOutputFile;
        private string _savedRestorePointsOutputFile;

        public BackupJobaFileSystemStateSaver(
            IBackupJobaConfigurationRepresenter configurationRepresenter,
            string configurationOutputFile,
            string savedRestorePointsOutputFile)
        {
            _configurationRepresenter = configurationRepresenter;
            _configurationOutputFile = configurationOutputFile;
            _savedRestorePointsOutputFile = savedRestorePointsOutputFile;
        }

        public void SaveState()
        {
            using (FileStream ofstream = File.Open(_configurationOutputFile, FileMode.OpenOrCreate))
            {
                ofstream.Write(_configurationRepresenter.GetConfigurationRepresentation());
            }

            using (FileStream ofstream = File.Open(_savedRestorePointsOutputFile, FileMode.OpenOrCreate))
            {
                ofstream.Write(_configurationRepresenter.GetSavedRestorePointsRepresentation());
            }
        }
    }
}
