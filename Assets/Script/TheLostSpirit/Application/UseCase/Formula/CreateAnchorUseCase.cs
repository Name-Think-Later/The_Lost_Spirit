using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class CreateAnchorUseCase : IUseCase<CreateAnchorUseCase.Output, Void>
    {
        readonly IAnchorInstanceFactory _anchorInstanceFactory;
        readonly AnchorRepository       _anchorRepository;
        readonly AnchorViewModelStore   _anchorViewModelStore;

        public CreateAnchorUseCase(
            IAnchorInstanceFactory anchorInstanceFactory,
            AnchorRepository       anchorRepository,
            AnchorViewModelStore   anchorViewModelStore
        ) {
            _anchorInstanceFactory = anchorInstanceFactory;
            _anchorRepository      = anchorRepository;
            _anchorViewModelStore  = anchorViewModelStore;
        }

        public Output Execute(Void input) {
            var instance = _anchorInstanceFactory.Create();
            _anchorRepository.Save(instance.Entity);
            _anchorViewModelStore.Save(instance.ViewModelReference);

            var anchorID = instance.Entity.ID;

            return new Output(anchorID);
        }

        public record struct Output(AnchorID AnchorID) : IOutput;
    }
}