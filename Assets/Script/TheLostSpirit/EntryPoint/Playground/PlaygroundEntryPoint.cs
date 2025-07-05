using Script.Circuit;
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

            var c1 = new CircuitNode(new Memory("c1"));
            var c2 = new CircuitNode(new Memory("c2"));
            c1.Adjacencies[0].Connect(c2.Adjacencies[1]);
            Debug.Log(c1.ToString());
            Debug.Log(c2.ToString());
        }
    }
}