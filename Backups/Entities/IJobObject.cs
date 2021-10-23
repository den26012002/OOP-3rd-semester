using System;

namespace Backups.Entities
{
    public interface IJobObject : IEquatable<IJobObject>
    {
        string Name { get; }
        string Extension { get; }
        byte[] GetRepresentation();
    }
}
