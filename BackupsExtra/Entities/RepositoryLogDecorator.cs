using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RepositoryLogDecorator : IRepository
    {
        private IRepository _repository;
        private ILogger _logger;

        public RepositoryLogDecorator(IRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IRepository OriginalRepository => _repository;
        public ILogger Logger => _logger;

        public void SaveRestorePoint(RestorePoint restorePoint)
        {
            _logger.Log($"Start saving restore point...");
            _repository.SaveRestorePoint(restorePoint);
            _logger.Log($"Restore point saved successfully!");
        }

        public void RemoveRestorePoint(uint restorePointId)
        {
            _logger.Log($"Start deleting restore point with id {restorePointId}...");
            _repository.RemoveRestorePoint(restorePointId);
            _logger.Log($"Restore point with id {restorePointId} deleted successfully!");
        }

        public void MergeRestorePoints(uint oldRestorePointId, uint newRestorePointId)
        {
            _logger.Log($"Start merging restore points with ids {oldRestorePointId} and {newRestorePointId}...");
            _repository.MergeRestorePoints(oldRestorePointId, newRestorePointId);
            _logger.Log($"Restore points with ids {oldRestorePointId} and {newRestorePointId} merged successfully!");
        }

        public List<IJobObject> GetJobObjects(uint restorePointId)
        {
            _logger.Log("Start unpacking saved job objects...");
            List<IJobObject> jobObjects = _repository.GetJobObjects(restorePointId);
            _logger.Log("Saved job objects unpacked successfully");
            return jobObjects;
        }
    }
}
