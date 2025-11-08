using MoreLinq;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
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


        PlaygroundContext Construct() {
            _userInputContext.Construct();
            _playerContext.Construct();
            _formulaContext.Construct(_userInputContext);

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
                createPortalByInstanceUseCase.Execute(new(ctx));
            });

            var formulaID = _formulaContext.FormulaViewModelStore[0].ID;

            var n1 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s1 = _formulaContext.CreateSkillUseCase.Execute(new(0)).SkillID;
            _formulaContext.NodeContainSkillUseCase.Execute(new(n1, s1));

            var n2 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n3 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;


            _formulaContext.FormulaAddNodeUseCase.Execute(new(formulaID, n1));
            _formulaContext.FormulaAddNodeUseCase.Execute(new(formulaID, n2));
            _formulaContext.FormulaAddNodeUseCase.Execute(new(formulaID, n3));


            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n1, 1), To: (n2, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n2, 1), To: (n3, 0)));
        }
    }
}