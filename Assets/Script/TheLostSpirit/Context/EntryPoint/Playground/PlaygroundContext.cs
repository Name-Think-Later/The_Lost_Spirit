using System.Linq;
using TheLostSpirit.Context.Formula;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Infrastructure;
using UnityEngine;

namespace TheLostSpirit.Context.EntryPoint.Playground
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
            AppScope.Initialize(new EventBus());

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

            var createPortalByInstanceUseCase = _portalContext.CreateByInstanceUseCase;
            var portalIdList =
                _testPortals
                    .Select(ctx => {
                        ctx.Construct();
                        var portalID = createPortalByInstanceUseCase.Execute(new(ctx)).PortalID;

                        return portalID;
                    })
                    .ToList();
            _portalContext.ConnectUseCase.Execute(new(portalIdList[0], portalIdList[1]));


            var formulaID = _formulaContext.FormulaViewModelStore[0].ID;

            var first  = new SkillConfigID("001");
            var second = new SkillConfigID("002");

            var n1 = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s1 = _formulaContext.skillContext.CreateSkillUseCase.Execute(new(first)).SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new(n1, s1));

            var n2 = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s2 = _formulaContext.skillContext.CreateSkillUseCase.Execute(new(second)).SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new(n2, s2));

            var n3 = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s3 = _formulaContext.skillContext.CreateSkillUseCase.Execute(new(second)).SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new(n3, s3));

            var n4 = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s4 = _formulaContext.skillContext.CreateSkillUseCase.Execute(new(second)).SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new(n4, s4));

            var n5 = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s5 = _formulaContext.skillContext.CreateSkillUseCase.Execute(new(second)).SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new(n5, s5));

            var n6 = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s6 = _formulaContext.skillContext.CreateSkillUseCase.Execute(new(second)).SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new(n6, s6));
            // var n7 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            // var n8 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            // var n9 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;


            _formulaContext.FormulaAddNodeUseCase.Execute(new(formulaID, n1));


            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new(From: (n1, 0), To: (n2, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new(From: (n1, 1), To: (n3, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new(From: (n2, 1), To: (n4, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new(From: (n2, 2), To: (n5, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new(From: (n5, 1), To: (n6, 0)));
            // _formulaContext.ConnectNodeUseCase.Execute(new(From: (n3, 1), To: (n7, 0)));
            // _formulaContext.ConnectNodeUseCase.Execute(new(From: (n4, 1), To: (n8, 0)));
            // _formulaContext.ConnectNodeUseCase.Execute(new(From: (n5, 2), To: (n9, 0)));

            //DFS 124563
            //BFS 123456
        }
    }
}