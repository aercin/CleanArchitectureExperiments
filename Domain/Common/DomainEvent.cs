using System;
using System.Collections.Generic;

namespace Domain.Common
{
    public interface IHasDomainEvent
    {
        public ICollection<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent
    {
        public DomainEvent()
        {
            DateOccured = DateTime.Now;
        }

        public bool IsPublished { get; set; }
        public DateTime DateOccured { get; protected set; }
    }
}
