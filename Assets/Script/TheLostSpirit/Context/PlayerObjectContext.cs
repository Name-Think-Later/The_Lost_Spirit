using Sirenix.OdinInspector;
using TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Identify;
using TheLostSpirit.View.Input;
using TheLostSpirit.ViewModel;
using UnityEngine;

namespace TheLostSpirit.Context {
    public class PlayerObjectContext : MonoBehaviour, IObjectContext<PlayerID> {
        [SerializeField]
        PlayerConfig _playerConfig;

        [SerializeField, ChildGameObjectsOnly]
        PlayerMono _mono;

        public InteractableRepository InteractableRepository { get; private set; }
        public PlayerViewModel PlayerViewModel { get; private set; }


        GeneralInputView _generalInputView;

        public void Construct(
            GeneralInputView generalInputView
        ) {
            _generalInputView = generalInputView;

            InteractableRepository = new InteractableRepository();
        }

        public PlayerID Produce() {
            var id = new PlayerID();

            PlayerEntity.Construct(id, _playerConfig, _mono);

            PlayerViewModel =
                new PlayerViewModel(
                    id,
                    new MoveInputUseCase(),
                    new DoJumpInputUseCase(),
                    new ReleaseJumpInputUseCase(),
                    new InteractInputUseCase(InteractableRepository)
                );

            _generalInputView.Bind(PlayerViewModel);

            return id;
        }
    }
}