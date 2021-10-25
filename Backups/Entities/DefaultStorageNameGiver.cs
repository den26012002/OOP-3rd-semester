namespace Backups.Entities
{
    public class DefaultStorageNameGiver : IStorageNameGiver
    {
        private uint _nextStorageId = 1;
        public string GiveName(Storage storage)
        {
            if (storage.JobObjects.Count == 1)
            {
                return storage.JobObjects[0].Name;
            }
            else
            {
                return "FileStorage" + _nextStorageId.ToString();
            }
        }
    }
}
