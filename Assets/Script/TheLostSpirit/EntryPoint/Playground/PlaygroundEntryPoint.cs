using Script.TheLostSpirit.CircuitSystem;
using Script.TheLostSpirit.Controller.PlayerController;
using Script.TheLostSpirit.Presenter.PlayerPresenter;
using Script.TheLostSpirit.Reference.PlayerReference;
using Script.TheLostSpirit.SkillSystem.SkillBase;
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

            var c1 = new Circuit.Node(new Skill(new Skill.Info("c1")));
            var c2 = new Circuit.Node(new Skill(new Skill.Info("c2")));
            var c3 = new Circuit.Node(new Skill(new Skill.Info("c3")));
            c1.Adjacencies[0].To(c2.Adjacencies[0]);
            c1.Adjacencies[1].To(c3.Adjacencies[0]);

            c1.Traversal();
        }
    }
}