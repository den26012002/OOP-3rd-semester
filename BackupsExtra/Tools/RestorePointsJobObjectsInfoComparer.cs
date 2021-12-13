using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Tools
{
    public class RestorePointsJobObjectsInfoComparer : IComparer<RestorePointJobObjectsInfo>
    {
        public int Compare(RestorePointJobObjectsInfo x, RestorePointJobObjectsInfo y)
        {
            if (x.Id == y.Id)
            {
                return 0;
            }

            return x.Id < y.Id ? -1 : 1;
        }
    }
}
