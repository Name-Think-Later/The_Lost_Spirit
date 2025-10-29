using MoreLinq;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Others.FormulaSystem;
using TheLostSpirit.Others.SkillSystem.CoreModule;
using TheLostSpirit.Others.SkillSystem.SkillBase;
using TheLostSpirit.Presentation.View.Input;
using TheLostSpirit.Presentation.ViewModel.Formula.InputHandler;
using TheLostSpirit.Presentation.ViewModel.Formula.OutputHandler;
using UnityEngine;

namespace TheLostSpirit.Context.Playground
{
    public class PlaygroundContext : MonoBehaviour
    {
        [SerializeField]
        PlayerContext _playerContext;

        [SerializeField]
        PlayerInstanceContext _playerInstanceContext;

        [SerializeField]
        UserInputContext _userInputContext;

        [SerializeField]
        PortalContext _portalContext;

        [SerializeField]
        FormulaContext _formulaContext;

        [SerializeField]
        PortalInstanceContext[] _testPortals;

        Formula[] _formulas;

        ActionMap        _actionMap;
        GeneralInputView _generalInputView;

        PlaygroundContext Construct() {
            _userInputContext.Construct();
            _playerContext.Construct();
            _formulaContext.Construct();

            _portalContext.Construct(_playerContext);

            return this;
        }

        void Awake() {
            Construct();

            TestBlock();
        }

        void TestBlock() {
            _playerInstanceContext.Construct(_playerContext, _userInputContext);

            var createPortalByInstanceUseCase = _portalContext.CreatePortalByInstanceUseCase;
            _testPortals.ForEach(ctx => {
                ctx.Construct();
                var createPortalInstanceInput = new CreatePortalByInstanceUseCase.Input(ctx);
                createPortalByInstanceUseCase.Execute(createPortalInstanceInput);
            });
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