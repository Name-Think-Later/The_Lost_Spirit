using System;

namespace TheLostSpirit.Infrastructure {
    public interface IIdentity {
        public Guid Value => Guid.NewGuid();
    }
}