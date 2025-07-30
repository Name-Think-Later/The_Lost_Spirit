using Script.TheLostSpirit.CircuitSystem;
using Script.TheLostSpirit.PlayerModule;
using Script.TheLostSpirit.SkillSystem.CoreModule;
using Script.TheLostSpirit.SkillSystem.CoreModule.CircuitActivator;
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
        Circuit[]        _circuits;

        void Awake() {
            _actionMap        = new ActionMap();
            _playerController = new PlayerController(_playerReference);
            _circuits = new[] {
                new Circuit(_actionMap.General.FirstCircuit),
                new Circuit(_actionMap.General.SecondCircuit)
            };

            _ = new GeneralActionsPresenter(_playerController, _actionMap.General, _playerReference);
            _ = new PlayerPhysicPresenter(_playerController, _playerReference);

            _actionMap.Enable();

            //Test code
            var singleClick   = new SingleClick();
            var coreInfo      = new Skill.Info("c1");
            var coreBehaviour = new Core.BehaviourData { CircuitActivator = singleClick };

            var coreModel = new Core.Model { Info = coreInfo, BehaviourData = coreBehaviour };
            var core      = new Core(coreModel);


            var c1 = new Circuit.SkillNode<Core>(core);
            var c2 = new Circuit.SkillNode<Skill>(new Skill(new Skill.Info("c2")));
            var c3 = new Circuit.SkillNode<Skill>(new Skill(new Skill.Info("c3")));
            var c4 = new Circuit.SkillNode<Skill>(new Skill(new Skill.Info("c4")));
            c1.Adjacencies[0].To(c2.Adjacencies[0]);
            c1.Adjacencies[1].To(c3.Adjacencies[0]);

            c2.Adjacencies[0].To(c4.Adjacencies[0]);

            _circuits[0].Add(c1);
            _circuits[1].Add(c1);
        }
    }
}