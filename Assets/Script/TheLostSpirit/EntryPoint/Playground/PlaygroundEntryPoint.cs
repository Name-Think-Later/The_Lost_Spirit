using TheLostSpirit.Application.UseCase.Input;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Portal;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.FormulaSystem;
using TheLostSpirit.Identify;
using TheLostSpirit.SkillSystem.CoreModule;
using TheLostSpirit.SkillSystem.CoreModule.InputHandler;
using TheLostSpirit.SkillSystem.CoreModule.OutputHandler;
using TheLostSpirit.SkillSystem.SkillBase;
using TheLostSpirit.View;
using TheLostSpirit.ViewModel;
using TheLostSpirit.ViewModel.Portal;
using UnityEngine;

namespace TheLostSpirit.EntryPoint.Playground {
    public class PlaygroundEntryPoint : MonoBehaviour {
        [SerializeField]
        [InlineEditor]
        PlayerConfig _playerConfig;

        [Required, SerializeField]
        [InlineEditor(InlineEditorModes.FullEditor)]
        PlayerMono _playerMono;

        [SerializeField]
        PortalMono _leftPortal;

        [SerializeField]
        PortalView _leftPortalView;

        [SerializeField]
        PortalMono _rightPortal;

        [SerializeField]
        PortalView _rightPortalView;

        ActionMap              _actionMap;
        PlayerEntity           _playerEntity;
        PlayerViewModel        _playerViewModel;
        InteractableRepository _interactableRepository;

        PortalRepository     _portalRepository;
        PortalViewModelStore _portalViewModelStore;
        PortalFactory        _portalFactory;

        Formula[] _formulas;

        void Awake() {
            InitPlayer();

            InitPortal();

            _portalFactory = new PortalFactory(_portalRepository, _portalViewModelStore);
            var leftID  = _portalFactory.Create(_leftPortal, _leftPortalView);
            var rightID = _portalFactory.Create(_rightPortal, _rightPortalView);

            var leftPortal  = _portalRepository.GetByID(leftID);
            var rightPortal = _portalRepository.GetByID(rightID);
            leftPortal.LinkTo(rightPortal.ID);
            rightPortal.LinkTo(leftPortal.ID);
        }

        void InitPortal() {
            _portalRepository     = new PortalRepository();
            _portalViewModelStore = new PortalViewModelStore();

            _ = new PortalInRangeEventHandler(_portalRepository, _interactableRepository, _playerEntity);
            _ = new PortalOutOfRangeEventHandler(_portalRepository, _interactableRepository, _playerEntity);
            _ = new PortalInFocusEventHandler(_portalViewModelStore);
            _ = new PortalOutOfFocusEventHandler(_portalViewModelStore);
            _ = new PortalTeleportEventHandler(_portalRepository, _playerEntity);
        }

        void InitPlayer() {
            _actionMap = new ActionMap();
            _actionMap.Enable();

            _interactableRepository = new InteractableRepository();

            var playerID = new PlayerID();
            _playerEntity = new PlayerEntity(playerID, _playerConfig, _playerMono);
            _playerViewModel = new PlayerViewModel(
                playerID,
                new MoveInputUseCase(_playerEntity),
                new InteractInputUseCase(_playerEntity, _interactableRepository)
            );


            var generalActions = _actionMap.General;
            _ = new PlayerInputBinding(generalActions, _playerViewModel);
        }

        void FormulaTest(ActionMap.GeneralActions generalActions) {
            _formulas = new[] {
                new Formula(),
                new Formula()
            };
            _ = new FormulaDefaultInputBinding(generalActions, _formulas);


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
        }
    }
}