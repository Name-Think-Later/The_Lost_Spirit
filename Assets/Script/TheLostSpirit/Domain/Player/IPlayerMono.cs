using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.Domain;

namespace TheLostSpirit.Domain.Player {
    public interface IPlayerMono : IEntityMono<PlayerID> {
        void SetMoveSpeed(float speed);

        void Jump(float speed, float jumpingGravityScale);
        void RestoreGravityScale();
    }
}