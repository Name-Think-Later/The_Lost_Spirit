using System;

namespace TheLostSpirit.Domain.Skill.Core {
    public interface IOutputPolicy {
        public void HandleOutput(Action output);
    }
}