public class PlayerPresenter {
    readonly ActionMap            _actionMap;
    readonly PlayerController     _playerController;
    readonly GeneralKeyMapActions _generalKeyMapActions;

    public PlayerPresenter(
        ActionMap        actionMap,
        PlayerController playerController
    ) {
        _actionMap            = actionMap;
        _playerController     = playerController;
        _generalKeyMapActions = new GeneralKeyMapActions(playerController);

        BundingAction();
    }

    private void BundingAction() {
        _actionMap
            .GeneralKeymap
            .SetCallbacks(_generalKeyMapActions);
    }
}