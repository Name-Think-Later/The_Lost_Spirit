using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Player
{
    public interface IPlayerMono : IEntityMono<PlayerID>
    {
        void SetMoveSpeed(float speed);

        void Jump(float speed, float jumpingGravityScale);

        void SetPosition(Vector2 position);

        void RestoreGravityScale();

        void Interact();
    }
}