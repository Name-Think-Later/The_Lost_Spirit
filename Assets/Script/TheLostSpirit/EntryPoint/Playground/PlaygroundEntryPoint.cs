using Script.TheLostSpirit.Controller.PlayerController;
using Script.TheLostSpirit.Presenter.PlayerPresenter;
using Script.TheLostSpirit.Reference.PlayerReference;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.EntryPoint.Playground {
    public class PlaygroundEntryPoint : MonoBehaviour {
        [Required, SerializeField]
        [InlineEditor(InlineEditorModes.FullEditor)]
        PlayerReference _playerReference;


        ActionMap        _actionMap;
        PlayerController _playerController;

        void Awake() {
            _actionMap        = new ActionMap();
            _playerController = new PlayerController(_playerReference);

            _ = new PlayerManipulatePresenter(_actionMap, _playerController, _playerReference);
        }
    }
}