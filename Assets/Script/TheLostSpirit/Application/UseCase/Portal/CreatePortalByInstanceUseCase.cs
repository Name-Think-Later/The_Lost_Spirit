using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.ViewModelReference.ViewModelStore;

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
            var portalEntity    = input.PortalInstanceContext.Entity;
            var portalViewModel = input.PortalInstanceContext.ViewModelReference;

            _portalRepository.Save(portalEntity);
            _portalViewModelStore.Save(portalViewModel);

            var portalID = portalEntity.ID;

            return new Output(portalID);
        }

        public record struct Input(IPortalInstanceContext PortalInstanceContext) : IInput;

        public record struct Output(PortalID PortalID) : IOutput;
    }
}