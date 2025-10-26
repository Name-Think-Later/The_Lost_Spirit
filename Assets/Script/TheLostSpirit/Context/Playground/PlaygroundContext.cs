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
        PlayerObjectContext _playerObjectContext;

        [SerializeField]
        PortalContext _portalContext;

        [SerializeField]
        FormulaContext _formulaContext;

        [SerializeField]
        PortalObjectContext[] _testPortals;

        Formula[] _formulas;

        ActionMap _actionMap;

        public void Construct() {
            _actionMap = new ActionMap();
            _actionMap.Enable();
            var generalInputView = new GeneralInputView(_actionMap.General);

            _playerObjectContext.Construct(generalInputView);
            _formulaContext.Construct(generalInputView);
            _portalContext.Construct(_playerObjectContext);
        }

        void Awake() {
            Construct();

            TestBlock();
        }

        void TestBlock() {
            _playerObjectContext.Instantiate();

            var createPortalByInstanceUseCase = _portalContext.CreatePortalByInstanceUseCase;
            _testPortals.ForEach(ctx => {
                ctx.Instantiate();
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