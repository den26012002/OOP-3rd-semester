using System.Linq;
using System.Text;
using Backups.Entities;

namespace Backups.Server.Entities
{
    public class UniversalJobObject : IJobObject
    {
        private string _representation;

        public UniversalJobObject(string name, string extension, string representation)
        {
            Name = name;
            Extension = extension;
            _representation = representation;
        }

        public string Name { get; }

        public string Extension { get; }

        public bool Equals(IJobObject other)
        {
            if (other is UniversalJobObject universalObject)
            {
                return universalObject.Name == Name &&
                    universalObject.Extension == Extension &&
                    Enumerable.SequenceEqual(universalObject.GetRepresentation(), GetRepresentation());
            }

            return false;
        }

        public byte[] GetRepresentation()
        {
            return Encoding.Default.GetBytes(_representation);
        }
    }
}
