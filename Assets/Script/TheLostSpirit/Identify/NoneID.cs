using System;

namespace TheLostSpirit.Identify
{
    public record struct NoneID : IIdentity
    {
        public static NoneID Default => new NoneID();
        public Guid Value => Guid.Empty;
    }
}