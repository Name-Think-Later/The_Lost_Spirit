using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill
{
    public abstract class SkillEntity : IEntity<ISkillID>
    {
        protected SkillEntity(ISkillID id) {
            ID = id;
        }

        public ISkillID ID { get; }

        public abstract UniTask Activate(FormulaPayload payload);
    }
}