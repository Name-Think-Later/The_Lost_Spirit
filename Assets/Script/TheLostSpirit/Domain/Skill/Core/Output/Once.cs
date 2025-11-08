using System;

namespace TheLostSpirit.Domain.Skill.Core.Output
{
    public class Once : IOutputPolicy
    {
        public void HandleOutput(Action output) {
            output();
        }
    }
}