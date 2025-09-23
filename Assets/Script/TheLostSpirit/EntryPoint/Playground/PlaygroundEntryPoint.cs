using System;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
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
        PlayerViewModel  _playerViewModel;
        Formula[]        _formulas;

        void Awake() {
            _actionMap        = new ActionMap();
            _playerController = new PlayerController(_playerReference);
            _playerViewModel  = new PlayerViewModel(_playerController);

            _formulas = new[] {
                new Formula(),
                new Formula()
            };

            var generalActions = _actionMap.General;
            _ = new PlayerInputBinding(generalActions, _playerViewModel).AddTo(_playerReference);
            _ = new FormulaDefaultInputBinding(generalActions, _formulas);

            _ = new PlayerUpdateBinding(_playerController).AddTo(_playerReference);


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

            var portalViewModelLeft  = new PortalViewModel(portalControllerLeft);
            var portalViewModelRight = new PortalViewModel(portalControllerRight);

            _ = new PortalInteractedBinding(_leftPortal, portalViewModelLeft);
            _ = new PortalInteractedBinding(_rightPortal, portalViewModelRight);
        }
    }

    public class PortalInteractedBinding {
        readonly PortalReference _portalReference;
        readonly PortalViewModel _portalViewModel;

        public PortalInteractedBinding(
            PortalReference portalReference,
            PortalViewModel portalViewModel
        ) {
            _portalReference = portalReference;
            _portalViewModel = portalViewModel;

            _portalReference
                .OnInteractAsObservable
                .Subscribe(_portalViewModel.OnInteracted);
        }
    }

    public class InteractDetectorTriggerBinding : IDisposable {
        readonly Collider2D       _interactDetector;
        readonly PlayerController _playerController;

        readonly IDisposable _disposable;

        public InteractDetectorTriggerBinding(
            Collider2D       interactDetector,
            PlayerController playerController
        ) {
            _interactDetector = interactDetector;
            _playerController = playerController;

            _interactDetector
                .OnTriggerEnter2DAsObservable()
                .Subscribe(collider => {
                    Debug.Log("Enter");

                    _playerController.SetInteractedTarget(collider);
                });
        }

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}