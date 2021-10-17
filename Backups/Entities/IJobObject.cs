namespace Backups.Entities
{
    public interface IJobObject
    {
        string Name { get; }
        string Extension { get; }
        byte[] GetRepresentation();
    }
}
