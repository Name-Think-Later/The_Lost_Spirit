using Script.TheLostSpirit.Application.ObjectContextContract;
using Sirenix.OdinInspector;
using TheLostSpirit.Context.Playground;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using TheLostSpirit.Presentation.ViewModel.Player;
using UnityEngine;

namespace TheLostSpirit.Context.Player
{
    public class PlayerInstanceContext
        : MonoBehaviour,
          IInstanceContext<PlayerID, PlayerEntity, IViewModelOnlyID<PlayerID>>
    {
        [SerializeField]
        PlayerConfig _playerConfig;

        [SerializeField, ChildGameObjectsOnly]
        PlayerMono _mono;

        public PlayerEntity Entity { get; private set; }
        public IViewModelOnlyID<PlayerID> ViewModelOnlyID { get; private set; }


        public PlayerInstanceContext Construct(
            PlayerContext    playerContext,
            UserInputContext userInputContext
        ) {
            var playerID = new PlayerID();

            Entity = PlayerEntity.Construct(playerID, _playerConfig, _mono);

            var viewModel =
                PlayerViewModel.Construct(
                    playerID,
                    playerContext.PlayerMoveUseCase,
                    playerContext.PlayerDoJumpUseCase,
                    playerContext.PlayerReleaseJumpUseCase,
                    playerContext.PlayerInteractUseCase
                );
            ViewModelOnlyID = viewModel;

            userInputContext.GeneralInputView.Bind(viewModel);

            return this;
        }
    }
}