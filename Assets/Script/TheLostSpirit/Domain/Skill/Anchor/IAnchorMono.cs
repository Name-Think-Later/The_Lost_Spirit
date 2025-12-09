using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Anchor
{
    public interface IAnchorMono : IEntityMono<AnchorID>
    {
        public void SetPosition(Vector2 position);
    }
}