namespace Backups.Entities
{
    public class EmptyJobObject : IJobObject
    {
        public string Name => string.Empty;

        public string Extension => string.Empty;

        public bool Equals(IJobObject other)
        {
            if (other is EmptyJobObject empty)
            {
                return empty.Name == Name &&
                    empty.Extension == Extension &&
                    empty.GetRepresentation() == GetRepresentation();
            }

            return false;
        }

        public byte[] GetRepresentation()
        {
            return new byte[0];
        }
    }
}
