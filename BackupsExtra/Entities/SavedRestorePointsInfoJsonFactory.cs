using System;
using System.Collections.Generic;
using System.Text.Json;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class SavedRestorePointsInfoJsonFactory : ISavedRestorePointsInfoFactory
    {
        private JsonElement _savedRestorePointsInfoJsonElement;

        public SavedRestorePointsInfoJsonFactory(JsonElement savedRestorePointsInfoJsonElement)
        {
            _savedRestorePointsInfoJsonElement = savedRestorePointsInfoJsonElement;
        }

        public List<RestorePointFileDirectoryInfo> GetRestorePointFileDirectoryInfos()
        {
            JsonElement.ArrayEnumerator savedRestorePointsInfoEnumerator =
                _savedRestorePointsInfoJsonElement.GetProperty("SavedRestorePoints").EnumerateArray();
            int arrayLength = _savedRestorePointsInfoJsonElement.GetProperty("SavedRestorePoints").GetArrayLength();
            var restorePointFileDirectoryInfos = new List<RestorePointFileDirectoryInfo>();
            for (int i = 0; i < arrayLength; ++i)
            {
                restorePointFileDirectoryInfos.Add(new RestorePointFileDirectoryInfo(
                    uint.Parse(savedRestorePointsInfoEnumerator.Current.GetProperty("RestorePointId").ToString()),
                    savedRestorePointsInfoEnumerator.Current.GetProperty("RestorePointDirectory").ToString()));
                savedRestorePointsInfoEnumerator.MoveNext();
            }

            return restorePointFileDirectoryInfos;
        }

        public List<RestorePointJobObjectsInfo> GetRestorePointJobObjectsInfos()
        {
            JsonElement.ArrayEnumerator restorePointJobObjectsInfosEnumerator =
                _savedRestorePointsInfoJsonElement.GetProperty("RestorePointJobObjectsInfos").EnumerateArray();
            int arrayLength = _savedRestorePointsInfoJsonElement.GetProperty("RestorePointJobObjectsInfos").GetArrayLength();
            var restorePointsInfos = new List<RestorePointJobObjectsInfo>();
            for (int i = 0; i < arrayLength; ++i)
            {
                JsonElement.ArrayEnumerator fullJobObjectsNamesEnumerator =
                    restorePointJobObjectsInfosEnumerator.Current.GetProperty("FullJobObjectsNames").EnumerateArray();
                int jobObjectNamesArrayLength =
                    restorePointJobObjectsInfosEnumerator.Current.GetProperty("FullJobObjectsNames").GetArrayLength();
                var fullJobObjectsNames = new List<string>();
                for (int j = 0; j < jobObjectNamesArrayLength; ++j)
                {
                    fullJobObjectsNames.Add(fullJobObjectsNamesEnumerator.Current.ToString());
                }

                restorePointsInfos.Add(new RestorePointJobObjectsInfo(
                    uint.Parse(restorePointJobObjectsInfosEnumerator.Current.GetProperty("Id").ToString()),
                    DateTime.Parse(restorePointJobObjectsInfosEnumerator.Current.GetProperty("DateOfCreation").ToString()),
                    fullJobObjectsNames));
                restorePointJobObjectsInfosEnumerator.MoveNext();
            }

            return restorePointsInfos;
        }
    }
}
