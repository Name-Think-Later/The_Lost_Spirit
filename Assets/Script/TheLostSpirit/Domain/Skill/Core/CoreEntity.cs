using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill.Core
{
    public class CoreEntity : SkillEntity, IEntity<CoreID>
    {
        readonly Core _core;
        public new CoreID ID { get; private set; }

        public IInputPolicy InputPolicy => _core.InputPolicy;
        public IOutputPolicy OutputPolicy => _core.OutputPolicy;

        public CoreEntity(CoreID id, CoreConfig config) : base(id) {
            ID = id;

            _core = new Core(config);
        }
    }
}