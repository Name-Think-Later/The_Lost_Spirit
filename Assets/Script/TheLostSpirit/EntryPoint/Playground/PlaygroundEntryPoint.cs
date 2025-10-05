using Script.TheLostSpirit.Application.UseCase.Input;
using TheLostSpirit.Application.UseCase.Input;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Portal;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.FormulaSystem;
using TheLostSpirit.SkillSystem.CoreModule;
using TheLostSpirit.SkillSystem.CoreModule.InputHandler;
using TheLostSpirit.SkillSystem.CoreModule.OutputHandler;
using TheLostSpirit.SkillSystem.SkillBase;
using TheLostSpirit.ViewModel;
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
        PortalMono _portalMono;

        [SerializeField]
        PortalMono _leftPortal;

        [SerializeField]
        PortalMono _rightPortal;

        ActionMap       _actionMap;
        Player          _player;
        PlayerEntity    _playerEntity;
        PlayerViewModel _playerViewModel;

        PortalRepository       _portalRepository;
        InteractableRepository _interactableRepository;

        Formula[] _formulas;

        void Awake() {
            _actionMap = new ActionMap();

            _interactableRepository = new InteractableRepository();

            _playerEntity = new PlayerEntity(_playerConfig, _playerMono);

            _playerViewModel = new PlayerViewModel(
                new MoveInputUseCase(_playerEntity),
                new InteractInputUseCase(_playerEntity, _interactableRepository)
            );


            var generalActions = _actionMap.General;
            _ = new PlayerInputBinding(generalActions, _playerViewModel);


            _actionMap.Enable();

            //FormulaTest(generalActions);

            _portalRepository = new PortalRepository();

            var leftPortal  = new PortalEntity(_leftPortal);
            var rightPortal = new PortalEntity(_rightPortal);
            leftPortal.LinkTo(rightPortal.ID);
            rightPortal.LinkTo(leftPortal.ID);

            _portalRepository.Add(leftPortal);
            _portalRepository.Add(rightPortal);


            _ = new PortalInRangeEventHandler(_portalRepository, _interactableRepository);
            _ = new PortalOutOfRangeEventHandler(_portalRepository, _interactableRepository);
            _ = new PortalTeleportEventHandler(_portalRepository, _playerEntity);
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