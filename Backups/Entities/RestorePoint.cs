using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private List<Storage> _storages;
        internal RestorePoint(uint id, List<Storage> storages)
        {
            Id = id;
            CreationDateTime = DateTime.Now;
            _storages = storages;
        }

        public uint Id { get; }
        public DateTime CreationDateTime { get; }
        public IReadOnlyList<Storage> Storages => _storages;
    }
}
