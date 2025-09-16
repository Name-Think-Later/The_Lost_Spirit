using System;
using R3;
using Script.TheLostSpirit.FormulaSystem;
using Script.TheLostSpirit.MapSystem;
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

        [SerializeField]
        PortalReference _portalReference;

        [SerializeField]
        PortalReference _leftPortal;

        [SerializeField]
        PortalReference _rightPortal;

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
            var outputHandler = new Once();

            var coreInfo = new Info { Name = "c1" };

            var coreBehaviour = new CoreBehaviourData { InputHandler = inputHandler, OutputHandler = outputHandler };

            var coreModel = new CoreModel { Info = coreInfo, BehaviourData = coreBehaviour };
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


            //portalTest
            var portalControllerLeft  = new PortalController(_leftPortal);
            var portalControllerRight = new PortalController(_rightPortal);
            portalControllerLeft.Connect(portalControllerRight);

            Observable
                .EveryUpdate()
                .Subscribe(_ => {
                    portalControllerLeft.DebugPortal();
                    portalControllerRight.DebugPortal();
                });
        }
    }
}