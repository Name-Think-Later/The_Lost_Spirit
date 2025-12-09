using System;

namespace TheLostSpirit.Identity.EntityID
{
    public abstract record RuntimeID<TRuntimeID> : IRuntimeID
        where TRuntimeID : RuntimeID<TRuntimeID>, new()
    {
        public static TRuntimeID Empty => new TRuntimeID { Value = Guid.Empty };
        public Guid Value { get; private set; }

        public static TRuntimeID New() {
            return new TRuntimeID { Value = Guid.NewGuid() };
        }
    }
}