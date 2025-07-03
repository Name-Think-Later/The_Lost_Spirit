using UnityEngine.InputSystem;

public class GeneralKeyMapActions : ActionMap.IGeneralKeymapActions {
    readonly PlayerController _playerController;

    public GeneralKeyMapActions(PlayerController playerController) {
        _playerController = playerController;
    }

    public void OnMove(InputAction.CallbackContext context) { }
}