using System.Collections.Generic;
using System.IO;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class FileSystemRestorePointsRestorer : IRestorePointsRestorer
    {
        public void RestoreJobObjectsToDifferentLocation(List<IJobObject> jobObjects, string location)
        {
            foreach (IJobObject jobObject in jobObjects)
            {
                using (var ofstream = new FileStream(location + "\\" + jobObject.Name + jobObject.Extension, FileMode.OpenOrCreate))
                {
                    ofstream.Write(jobObject.GetRepresentation());
                }
            }
        }

        public void RestoreJobObjectsToOriginalLocation(List<IJobObject> jobObjects, RestorePointJobObjectsInfo restorePointJobObjectsInfo)
        {
            var fileInfos = new List<FileInfo>();
            foreach (string fullJobObjectName in restorePointJobObjectsInfo.FullJobObjectsNames)
            {
                fileInfos.Add(new FileInfo(fullJobObjectName));
            }

            foreach (IJobObject jobObject in jobObjects)
            {
                using (var ofstream = new FileStream(fileInfos.Find(info => info.Name == (jobObject.Name + jobObject.Extension)).FullName, FileMode.OpenOrCreate))
                {
                    ofstream.Write(jobObject.GetRepresentation());
                }
            }
        }
    }
}
