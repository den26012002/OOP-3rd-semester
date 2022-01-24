using System;
using System.Collections.Generic;

namespace Reports.Core.Entities
{
    public class Employee
    {
        public Employee(string name, Guid? bossId = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            BossId = bossId;
        }

        public Guid Id { get; private init; }
        public string Name { get; private init; }
        public Guid? BossId { get; private set; }

        public void UpdateBoss(Guid newBossId)
        {
            BossId = newBossId;
        }
    }
}
