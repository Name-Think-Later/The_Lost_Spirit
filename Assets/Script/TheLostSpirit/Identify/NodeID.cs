using System;

namespace TheLostSpirit.Identify
{
    public record struct NodeID : IIdentity
    {
        static int _index = 1;
        public static NodeID New() => new NodeID { Index = _index++, Value = Guid.NewGuid() };
        public int Index { get; private set; }
        public Guid Value { get; private set; }
    }
}