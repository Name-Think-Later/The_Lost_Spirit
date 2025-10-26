using R3;
using Script.TheLostSpirit.Application.ObjectContextContract;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using TheLostSpirit.Presentation.View.Input;
using TheLostSpirit.Presentation.ViewModel.Player;
using UnityEngine;

namespace TheLostSpirit.Context.Player
{
    public class PlayerObjectContext
        : MonoBehaviour,
          IObjectContext<PlayerID, PlayerEntity, IViewModelOnlyID<PlayerID>>
    {
        [SerializeField]
        PlayerConfig _playerConfig;

        [SerializeField, ChildGameObjectsOnly]
        PlayerMono _mono;

        public InteractableRepository InteractableRepository { get; private set; }

        public PlayerEntity Entity { get; private set; }
        public IViewModelOnlyID<PlayerID> ViewModelOnlyID { get; private set; }


        public PlayerMoveUseCase PlayerMoveUseCase { get; private set; }
        public PlayerDoJumpUseCase PlayerDoJumpUseCase { get; private set; }
        public PlayerReleaseJumpUseCase PlayerReleaseJumpUseCase { get; private set; }
        public PlayerInteractUseCase PlayerInteractUseCase { get; private set; }


        GeneralInputView _generalInputView;

        public void Construct(GeneralInputView generalInputView) {
            _generalInputView = generalInputView;
            _generalInputView.AddTo(this);

            InteractableRepository = new InteractableRepository();

            PlayerMoveUseCase        = new PlayerMoveUseCase();
            PlayerDoJumpUseCase      = new PlayerDoJumpUseCase();
            PlayerReleaseJumpUseCase = new PlayerReleaseJumpUseCase();
            PlayerInteractUseCase    = new PlayerInteractUseCase(InteractableRepository);
        }


        public PlayerObjectContext Instantiate() {
            var id = new PlayerID();

            Entity = PlayerEntity.Construct(id, _playerConfig, _mono);

            var viewModel =
                new PlayerViewModel(
                    id,
                    PlayerMoveUseCase,
                    PlayerDoJumpUseCase,
                    PlayerReleaseJumpUseCase,
                    PlayerInteractUseCase
                );

            ViewModelOnlyID = viewModel;

            _generalInputView.Bind(viewModel);

            return this;
        }
    }
}