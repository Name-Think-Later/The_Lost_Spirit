using Script.TheLostSpirit.FormulaSystem;
using Script.TheLostSpirit.PlayerModule;
using Script.TheLostSpirit.SkillSystem.CoreModule;
using Script.TheLostSpirit.SkillSystem.CoreModule.InputHandler;
using Script.TheLostSpirit.SkillSystem.CoreModule.OutputHandler;
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
        Formula[]        _formulas;

        void Awake() {
            _actionMap        = new ActionMap();
            _playerController = new PlayerController(_playerReference);

            _formulas = new[] {
                new Formula(),
                new Formula()
            };

            _ = new PlayerControlInputEventHandler(_actionMap.General, _playerController, _playerReference);
            _ = new FormulaDefaultInputBinding(_actionMap.General, _formulas);

            _actionMap.Enable();

            //Test code
            var inputHandler  = new SingleClick();
            var outputHandler = new Repeat { RepeatTimes = 3, Interval = 0.5f };

            var coreInfo = new Info { Name = "c1" };

            var coreBehaviour = new Core.BehaviourData { InputHandler = inputHandler, OutputHandler = outputHandler };

            var coreModel = new Core.Model { Info = coreInfo, BehaviourData = coreBehaviour };
            var core      = new Core(coreModel);


            var c1 = new SkillNode<Core>(core);
            var c2 = new SkillNode<Skill>(new Skill(new Info { Name = "c2" }));
            var c3 = new SkillNode<Skill>(new Skill(new Info { Name = "c3" }));
            var c4 = new SkillNode<Skill>(new Skill(new Info { Name = "c4" }));
            c1.Adjacencies[0].To(c2.Adjacencies[0]);
            c1.Adjacencies[1].To(c3.Adjacencies[0]);

            c2.Adjacencies[0].To(c4.Adjacencies[0]);

            _formulas[0].Add(c1);
            _formulas[1].Add(c1);
        }
    }
}