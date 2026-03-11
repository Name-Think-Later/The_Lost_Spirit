using System;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;
using TheLostSpirit.Presentation.ViewModel.ViewModelReference.ViewModelStore;
using UnityEngine;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class CreateAnchorUseCase : IUseCase<CreateAnchorUseCase.Output, CreateAnchorUseCase.Input>
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

        public Output Execute(Input input) {
            var instance = _anchorInstanceFactory.Create(input.Position, input.Rotation, input.FormulaStreamID);
            _anchorRepository.Save(instance.Entity);
            _anchorViewModelStore.Save(instance.ViewModelReference);

            var anchorID = instance.Entity.ID;

            return new Output(anchorID);
        }

        public record struct Input(Vector2 Position, Vector2 Rotation, Guid FormulaStreamID) : IInput;

        public record struct Output(AnchorID AnchorID) : IOutput;
    }
}