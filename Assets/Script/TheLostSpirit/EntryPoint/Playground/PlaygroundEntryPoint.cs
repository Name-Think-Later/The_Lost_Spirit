using Script.TheLostSpirit.CircuitSystem;
using Script.TheLostSpirit.PlayerModule;
using Script.TheLostSpirit.SkillSystem.CoreModule;
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

            _ = new GeneralActionsPresenter(_playerController, _actionMap.General, _playerReference);
            _ = new PlayerPhysicPresenter(_playerController, _playerReference);


            _actionMap.Enable();


            var core = new Core(new Core.Model(new Skill.Info("c1"), new Core.BehaviourData()));
            var c1   = new Circuit.Node<Core>(core);
            var c2   = new Circuit.Node<Skill>(new Skill(new Skill.Info("c2")));
            var c3   = new Circuit.Node<Skill>(new Skill(new Skill.Info("c3")));
            c1.Adjacencies[0].To(c2.Adjacencies[0]);
            c1.Adjacencies[1].To(c3.Adjacencies[0]);

            c1.Activate();
        }
    }
}