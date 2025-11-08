using System;

namespace TheLostSpirit.Identify
{
    public record struct PlayerID : IIdentity
    {
        public static PlayerID New() => new PlayerID { Value = Guid.NewGuid() };
        public Guid Value { get; private set; }
    }
}