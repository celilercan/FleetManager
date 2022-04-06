using System;

namespace FleetManager.Data.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}
