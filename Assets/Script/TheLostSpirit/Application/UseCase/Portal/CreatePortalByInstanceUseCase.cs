using Script.TheLostSpirit.Application.ObjectContextContract;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.UseCase.Portal
{
    public class CreatePortalByInstanceUseCase
        : IUseCase<CreatePortalByInstanceUseCase.Output, CreatePortalByInstanceUseCase.Input>
    {
        readonly PortalRepository     _portalRepository;
        readonly PortalViewModelStore _portalViewModelStore;

        public CreatePortalByInstanceUseCase(
            PortalRepository     portalRepository,
            PortalViewModelStore portalViewModelStore
        ) {
            _portalRepository     = portalRepository;
            _portalViewModelStore = portalViewModelStore;
        }

        public Output Execute(Input input) {
            var portalEntity    = input.PortalObjectContext.Entity;
            var portalViewModel = input.PortalObjectContext.ViewModelOnlyID;

            _portalRepository.Save(portalEntity);
            _portalViewModelStore.Save(portalViewModel);

            var portalID = portalEntity.ID;

            return new Output(portalID);
        }

        public record struct Input(IPortalObjectContext PortalObjectContext) : IInput;

        public record struct Output(PortalID PortalID) : IOutput;
    }
}