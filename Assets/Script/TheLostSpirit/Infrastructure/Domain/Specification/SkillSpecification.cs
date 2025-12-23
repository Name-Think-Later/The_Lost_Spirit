using Sirenix.OdinInspector;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identity.SpecificationID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.Specification
{
    public abstract class SkillSpecification
        : ScriptableObject, ISpecification<SkillSpecificationID>, ISearchFilterable
    {
        [SerializeField]
        SkillSpecificationID _id;

        public SkillSpecificationID ID => _id;
        public abstract SkillConfig Config { get; }

#if UNITY_EDITOR
        public bool IsMatch(string searchString) {
            var matchId   = !string.IsNullOrEmpty(ID.Value) && ID.Value.Contains(searchString);
            var matchName = !string.IsNullOrEmpty(Config.name) && Config.name.Contains(searchString);

            return matchId || matchName;
        }
#endif
    }
}