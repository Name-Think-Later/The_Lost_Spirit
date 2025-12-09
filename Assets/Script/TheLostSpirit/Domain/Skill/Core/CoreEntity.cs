using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.FormulaIOPolicy;
using TheLostSpirit.Domain.Skill.Core.Event;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Core
{
    public class CoreEntity : SkillEntity, IEntity<CoreID>
    {
        readonly Core      _core;
        readonly IEventBus _eventBus;

        public CoreEntity(CoreID id, CoreConfig config) : base(id) {
            ID        = id;
            _eventBus = AppScope.EventBus;
            _core     = new Core(config);

            FormulaIOPolicy = new FormulaIOPolicy(_core.InputPolicy, _core.OutputPolicy);
        }

        public IFormulaIOPolicy FormulaIOPolicy { get; }
        public new CoreID ID { get; }

        public override async UniTask Activate(FormulaPayload payload) {
            //create Anchor
            var coreActivated = new AsyncCoreActivatedEvent(payload);
            await _eventBus.PublishAsync(coreActivated);
        }
    }
}