using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class StorageAlgorithmLogDecorator : IStorageAlgorithm
    {
        private IStorageAlgorithm _storageAlgorithm;
        private ILogger _logger;

        public StorageAlgorithmLogDecorator(IStorageAlgorithm storageAlgorithm, ILogger logger)
        {
            _storageAlgorithm = storageAlgorithm;
            _logger = logger;
        }

        public List<Storage> OrganizeJobObjects(List<IJobObject> jobObjects)
        {
            _logger.Log($"Start organizing job objects...");
            List<Storage> jobObjectGroups = _storageAlgorithm.OrganizeJobObjects(jobObjects);
            _logger.Log($"Job objects organized successfully!");
            return jobObjectGroups;
        }
    }
}
