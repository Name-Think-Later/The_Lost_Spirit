using System;

namespace TheLostSpirit.Identify
{
    public record NoneID : IIdentity
    {
        private NoneID() { }

        public Guid Value => Guid.Empty;

        public static NoneID Default => new NoneID();
    }
}