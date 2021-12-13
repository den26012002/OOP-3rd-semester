using System.Collections.Generic;
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

        public List<Storage> LoadStorages(string storageDirectoryPath)
        {
            var directoryInfo = new DirectoryInfo(storageDirectoryPath);
            FileInfo[] files = directoryInfo.GetFiles();
            var storages = new List<Storage>();
            foreach (FileInfo file in files)
            {
                using (var zipToRead = new FileStream($"{storageDirectoryPath}\\{file.Name}", FileMode.Open))
                {
                    using (var archive = new ZipArchive(zipToRead, ZipArchiveMode.Read))
                    {
                        var storageJobObjects = new List<IJobObject>();
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            using (var reader = new StreamReader(entry.Open()))
                            {
                                string fileContent = reader.ReadToEnd();
                                storageJobObjects.Add(new UniversalJobObject(entry.Name, string.Empty, fileContent));
                            }
                        }

                        storages.Add(new Storage(storageJobObjects));
                    }
                }
            }

            return storages;
        }
    }
}
