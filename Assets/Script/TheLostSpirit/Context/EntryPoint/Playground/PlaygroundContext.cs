using System.Linq;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Context.Formula;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Identity.SpecificationID;
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

        void Awake() {
            Construct();

            TestBlock();
        }


        PlaygroundContext Construct() {
            AppScope.Initialize(new EventBus());

            _userInputContext.Construct();
            _playerContext.Construct();
            _formulaContext.Construct(_userInputContext);

            _portalContext.Construct(_playerContext);

            return this;
        }

        void TestBlock() {
            _playerInstanceContext.Construct(_playerContext, _userInputContext);

            var createPortalByInstanceUseCase = _portalContext.CreateByInstanceUseCase;
            var portalIdList =
                _testPortals
                    .Select(ctx => {
                        ctx.Construct();
                        var portalID =
                            createPortalByInstanceUseCase
                                .Execute(new CreatePortalByInstanceUseCase.Input(ctx))
                                .PortalID;

                        return portalID;
                    })
                    .ToList();
            _portalContext.ConnectUseCase.Execute(new ConnectPortalUseCase.Input(portalIdList[0], portalIdList[1]));


            var formulaID = _formulaContext.FormulaViewModelStore[0].ID;

            var core     = new SkillSpecificationID("001");
            var manifest = new SkillSpecificationID("002");
            var weave    = new SkillSpecificationID("003");

            var (c, s1)  = SkillWithNode(core);
            var (m1, s2) = SkillWithNode(manifest);
            var (m2, s3) = SkillWithNode(manifest);
            var (w1, s4) = SkillWithNode(weave);
            var (w2, s5) = SkillWithNode(weave);
            var (w3, _) = SkillWithNode(weave);
            var (m3, s6) = SkillWithNode(manifest);
            var (m4, s7) = SkillWithNode(manifest);
            var (m5, _) = SkillWithNode(manifest);



            _formulaContext.FormulaAddNodeUseCase.Execute(new FormulaAddNodeUseCase.Input(formulaID, c));

            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((c, 0), (m1, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((c, 1), (m2, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((m1, 1), (w1, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((m1, 2), (w2, 0)));
            _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((w1, 1), (m3, 0)));
            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((m3, 1), (w2, 0)));
            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((w2, 1), (m5, 0)));
            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((m5, 1), (w3, 0)));
            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((w3, 1), (m4, 0)));
            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((w1, 2), (m4, 0)));

            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((n2, 2), (n5, 0)));
            // _formulaContext.nodeContext.ConnectNodeUseCase.Execute(new ConnectNodeUseCase.Input((n5, 1), (n6, 0)));
            // _formulaContext.ConnectNodeUseCase.Execute(new(From: (n3, 1), To: (n7, 0)));
            // _formulaContext.ConnectNodeUseCase.Execute(new(From: (n4, 1), To: (n8, 0)));
            // _formulaContext.ConnectNodeUseCase.Execute(new(From: (n5, 2), To: (n9, 0)));

            //DFS 124563
            //BFS 123456
        }

        (NodeID, ISkillID) SkillWithNode(SkillSpecificationID id) {
            var node = _formulaContext.nodeContext.CreateNodeUseCase.Execute(new CreateNodeUseCase.Input(3)).NodeID;
            var skill = _formulaContext.skillContext.CreateSkillUseCase.Execute(new CreateSkillUseCase.Input(id))
                                       .SkillID;
            _formulaContext.nodeContext.NodeContainSkillUseCase.Execute(new NodeContainSkillUseCase.Input(node, skill));

            return (node, skill);
        }
    }
}