namespace Backups.Entities
{
    public class EmptyJobObject : IJobObject
    {
        public string Name => string.Empty;

        public string Extension => string.Empty;

        public byte[] GetRepresentation()
        {
            return new byte[0];
        }
    }
}
