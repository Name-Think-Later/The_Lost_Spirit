using System;

namespace TheLostSpirit.Identify {
    public interface IIdentity {
        public Guid Value => Guid.NewGuid();
    }
}