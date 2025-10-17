using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Portal {
    public class ConnectPortalUseCase : IUseCase<Void, ConnectPortalUseCase.Input> {
        readonly PortalRepository _portalRepository;

        public ConnectPortalUseCase(
            PortalRepository portalRepository
        ) {
            _portalRepository = portalRepository;
        }

        public Void Execute(Input input) {
            var leftID  = input.LeftID;
            var rightID = input.RightID;

            var leftEntity  = _portalRepository.GetByID(leftID);
            var rightEntity = _portalRepository.GetByID(rightID);

            leftEntity.Associate(rightID);
            rightEntity.Associate(leftID);

            return Void.Default;
        }

        public record struct Input(PortalID LeftID, PortalID RightID) : IInput;
    }
}