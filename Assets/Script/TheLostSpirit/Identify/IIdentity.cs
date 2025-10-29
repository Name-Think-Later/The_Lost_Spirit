using System;

namespace TheLostSpirit.Identify {
    public interface IIdentity {
        Guid Value => Guid.NewGuid();
    }
}