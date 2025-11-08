using Script.TheLostSpirit.Domain.ViewModelPort;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill.Core
{
    public class CoreEntity : SkillEntity, IEntity<CoreID>
    {
        readonly Core _core;
        public new CoreID ID { get; private set; }

        public IFormulaIOPolicy FormulaIOPolicy => new FormulaIOPolicy(_core.InputPolicy, _core.OutputPolicy);

        public CoreEntity(CoreID id, CoreConfig config) : base(id) {
            ID = id;

            _core = new Core(config);
        }
    }
}