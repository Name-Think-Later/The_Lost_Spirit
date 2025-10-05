using System;

namespace TheLostSpirit.Infrastructure.DomainDriven {
    public interface IEntityID {
        public Guid Value => Guid.Empty;
    }
}