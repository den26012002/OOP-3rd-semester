namespace Backups.Entities
{
    public class MemoryJobObject
    {
        public MemoryJobObject(string fullName, byte[] representation)
        {
            FullName = fullName;
            Representation = representation;
        }

        public string FullName { get; }
        public byte[] Representation { get; }
    }
}
