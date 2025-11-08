using System;

namespace TheLostSpirit.Identify
{
    public record struct FormulaID : IIdentity
    {
        public static FormulaID New() => new FormulaID { Value = Guid.NewGuid() };
        public Guid Value { get; private set; }
    }
}