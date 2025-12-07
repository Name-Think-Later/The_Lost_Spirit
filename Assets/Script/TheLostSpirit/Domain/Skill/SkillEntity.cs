using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill
{
    public abstract class SkillEntity : IEntity<ISkillID>
    {
        public ISkillID ID { get; }

        protected Dictionary<PayloadSeq, FormulaPayload> payloadCache;

        protected SkillEntity(ISkillID id) {
            ID = id;

            payloadCache = new Dictionary<PayloadSeq, FormulaPayload>();
        }

        public abstract UniTask Activate(FormulaPayload payload);
    }
}