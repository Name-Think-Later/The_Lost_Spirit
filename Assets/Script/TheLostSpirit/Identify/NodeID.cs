using System;

namespace TheLostSpirit.Identify
{
    public record struct NodeID : IIdentity
    {
        public static NodeID New() => new NodeID { Value = Guid.NewGuid() };
        public Guid Value { get; private set; }
    }
}