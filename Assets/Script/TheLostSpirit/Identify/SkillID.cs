using System;

namespace TheLostSpirit.Identify
{
    public abstract record SkillID : IIdentity
    {
        public Guid Value { get; protected set; }
    }
}