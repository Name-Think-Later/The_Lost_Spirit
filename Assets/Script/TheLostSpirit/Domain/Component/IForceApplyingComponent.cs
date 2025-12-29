using UnityEngine;

namespace TheLostSpirit.Domain.Component
{
    public interface IForceApplyingComponent
    {
        public void ApplyForce(Vector2 force);
    }
}