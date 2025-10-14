using MoreLinq;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.FormulaSystem;
using TheLostSpirit.SkillSystem.CoreModule;
using TheLostSpirit.SkillSystem.CoreModule.InputHandler;
using TheLostSpirit.SkillSystem.CoreModule.OutputHandler;
using TheLostSpirit.SkillSystem.SkillBase;
using TheLostSpirit.View.Input;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace TheLostSpirit.Context.Playground {
    public class PlaygroundContext : MonoBehaviour {
        [SerializeField]
        PlayerObjectContext _playerObjectContext;

        [SerializeField]
        PortalContext _portalContext;

        [SerializeField]
        PortalObjectContext[] _testPortals;

        Formula[] _formulas;

        ActionMap _actionMap;

        public void Construct() {
            _actionMap = new ActionMap();
            _actionMap.Enable();
            var generalInputView = new GeneralInputView(_actionMap.General);

            _playerObjectContext.Construct(generalInputView);
            _portalContext.Construct(_playerObjectContext);
        }
        
        void Awake() {
            Construct();

            TestBlock();
        }

        void TestBlock() {
            var repository     = _portalContext.PortalRepository;
            var viewModelStore = _portalContext.PortalViewModelStore;
            _testPortals.ForEach(ctx => {
                ctx.Construct(repository, viewModelStore);
            });

            _playerObjectContext.Produce();
            _testPortals.ForEach(ctx => ctx.Produce());
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