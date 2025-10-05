using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Domain.Player {
    public interface IPlayerMono : IEntityMono<PlayerID> {
        void SetMoveSpeed(float speed);
    }
}