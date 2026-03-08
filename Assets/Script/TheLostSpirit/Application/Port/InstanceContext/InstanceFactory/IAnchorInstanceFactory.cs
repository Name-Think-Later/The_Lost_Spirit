using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using UnityEngine;

namespace TheLostSpirit.Application.Port.InstanceContext.InstanceFactory
{
    public interface IAnchorInstanceFactory
    {
        IAnchorInstanceContext Create(Vector2 position, Vector2 rotation);
    }
}