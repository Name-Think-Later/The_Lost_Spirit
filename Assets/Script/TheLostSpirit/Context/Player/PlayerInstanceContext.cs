using Sirenix.OdinInspector;
using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Infrastructure.Domain.Specification;
using TheLostSpirit.Presentation.ViewModel.Player;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using UnityEngine;

namespace TheLostSpirit.Context.Player
{
    public class PlayerInstanceContext
        : MonoBehaviour,
          IInstanceContext<PlayerID, PlayerEntity, IViewModelReference<PlayerID>>
    {
        [SerializeField, InlineEditor]
        PlayerSpecification _playerSpecification;

        [SerializeField, ChildGameObjectsOnly]
        PlayerMono _mono;

        public PlayerEntity Entity { get; private set; }
        public IViewModelReference<PlayerID> ViewModelReference { get; private set; }


        public PlayerInstanceContext Construct(
            PlayerContext    playerContext,
            UserInputContext userInputContext
        ) {
            var playerID = PlayerID.New();

            Entity = PlayerEntity.Construct(playerID, _playerSpecification.Config, _mono, AppScope.EventBus);

            var viewModel =
                PlayerViewModel.Construct(
                    playerID,
                    playerContext.PlayerMoveUseCase,
                    playerContext.PlayerDoJumpUseCase,
                    playerContext.PlayerReleaseJumpUseCase,
                    playerContext.PlayerInteractUseCase
                );
            ViewModelReference = viewModel;

            userInputContext.GeneralInputView.Bind(viewModel);

            return this;
        }
    }
}