using System.Collections.Generic;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJoba
    {
        private uint _nextRestorePointId = 1;
        private List<IJobObject> _currentObjects;
        private IStorageAlgorithm _storageAlgorithm;
        private IRepository _repository;
        private List<RestorePointJobObjectsInfo> _createdRestorePointsJobObjectsInfo;

        public BackupJoba(string name, IStorageAlgorithm storageAlgorithm, IRepository repository)
        {
            Name = name;
            _currentObjects = new List<IJobObject>();
            _storageAlgorithm = storageAlgorithm;
            _repository = repository;
            _createdRestorePointsJobObjectsInfo = new List<RestorePointJobObjectsInfo>();
        }

        public string Name { get; }
        public IReadOnlyList<IJobObject> CurrentObjects => _currentObjects;
        public IStorageAlgorithm StorageAlgorithm => _storageAlgorithm;
        public IRepository Repository => _repository;
        public List<RestorePointJobObjectsInfo> CreatedRestorePoints => _createdRestorePointsJobObjectsInfo;

        public void AddJobObject(IJobObject jobObject)
        {
            _currentObjects.Add(jobObject);
        }

        public void RemoveJobObject(IJobObject jobObject)
        {
            if (!_currentObjects.Contains(jobObject))
            {
                throw new BackupsException($"Error: there is no jobObject {jobObject} in BackupJoba {this}");
            }

            _currentObjects.Remove(jobObject);
        }

        public void ProcessObjects()
        {
            List<Storage> storages = _storageAlgorithm.OrganizeJobObjects(_currentObjects);
            var restorePoint = new RestorePoint(_nextRestorePointId++, storages);
            _createdRestorePointsJobObjectsInfo.Add(new RestorePointJobObjectsInfo(_currentObjects));
            _repository.SaveRestorePoint(restorePoint);
        }
    }
}
