using System;

namespace TheLostSpirit.Identity.EntityID
{
    public record struct NoneID : IRuntimeID
    {
        public static NoneID Default => new NoneID();
        public Guid Value => Guid.Empty;
    }
}