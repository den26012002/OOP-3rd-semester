using System.IO;
using System.IO.Compression;
using System.Text;

namespace Backups.Entities
{
    public class ZipArchiver : IArchiver
    {
        public void SaveStorage(Storage storage, string storageDirectoryPath, string storageName)
        {
            using (var zipToOpen = new FileStream(storageDirectoryPath + "\\" + storageName + ".zip", FileMode.OpenOrCreate))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (IJobObject jobObject in storage.JobObjects)
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(jobObject.Name + jobObject.Extension);
                        using (var writer = new StreamWriter(readmeEntry.Open()))
                        {
                            writer.Write(Encoding.Default.GetString(jobObject.GetRepresentation()));
                        }
                    }
                }
            }
        }
    }
}
