using System;
using System.Text;

namespace BackupsExtra.Entities
{
    public class BackupJobaConsoleStateSaver : IBackupJobaStateSaver
    {
        private IBackupJobaConfigurationRepresenter _configurationRepresenter;

        public BackupJobaConsoleStateSaver(IBackupJobaConfigurationRepresenter configurationRepresenter)
        {
            _configurationRepresenter = configurationRepresenter;
        }

        public void SaveState()
        {
            Console.WriteLine(Encoding.Default.GetString(_configurationRepresenter.GetConfigurationRepresentation()));
            Console.WriteLine(Encoding.Default.GetString(_configurationRepresenter.GetSavedRestorePointsRepresentation()));
        }
    }
}
