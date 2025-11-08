using System;

namespace TheLostSpirit.Identify
{
    public record struct RoomID : IIdentity
    {
        public static RoomID New() => new RoomID { Value = Guid.NewGuid() };
        public Guid Value { get; private set; }
    }
}