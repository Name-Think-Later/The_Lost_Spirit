using System;

namespace TheLostSpirit.Identify
{
    public record CoreID : SkillID
    {
        public static CoreID New() => new CoreID { Value = Guid.NewGuid() };
    }
}