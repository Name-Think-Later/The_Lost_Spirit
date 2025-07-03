using R3;

public class PlayerPresenter {
    readonly ActionMap            _actionMap;
    readonly PlayerController     _playerController;
    readonly PlayerReference      _playerReference;
    readonly GeneralKeyMapActions _generalKeyMapActions;

    public PlayerPresenter(
        ActionMap        actionMap,
        PlayerController playerController,
        PlayerReference  playerReference
    ) {
        _actionMap            = actionMap;
        _playerController     = playerController;
        _playerReference      = playerReference;
        _generalKeyMapActions = new GeneralKeyMapActions(playerController);

        ActionBinding();
        PlayerMovementUpdateBinding();
    }

    private void ActionBinding() {
        _actionMap
            .GeneralKeymap
            .SetCallbacks(_generalKeyMapActions);
        _actionMap.Enable();
    }

    private void PlayerMovementUpdateBinding() {
        Observable
            .EveryUpdate(UnityFrameProvider.FixedUpdate)
            .Subscribe(_ => {
                _playerController.ApplyVelocity();
            })
            .AddTo(_playerReference);
    }
}