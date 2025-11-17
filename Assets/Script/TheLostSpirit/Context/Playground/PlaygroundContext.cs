using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;
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
            var portalIdList =
                _testPortals
                    .Select(ctx => {
                        ctx.Construct();
                        var portalID = createPortalByInstanceUseCase.Execute(new(ctx)).PortalID;

                        return portalID;
                    })
                    .ToList();
            _portalContext.ConnectPortalUseCase.Execute(new(portalIdList[0], portalIdList[1]));


            var formulaID = _formulaContext.FormulaViewModelStore[0].ID;

            var n1 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var s1 = _formulaContext.CreateSkillUseCase.Execute(new(0)).SkillID;
            _formulaContext.NodeContainSkillUseCase.Execute(new(n1, s1));

            var n2 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n3 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n4 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n5 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n6 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n7 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n8 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;
            var n9 = _formulaContext.CreateNodeUseCase.Execute(new(3)).NodeID;


            _formulaContext.AddNodeInFormulaUseCase.Execute(new(formulaID, n1));


            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n1, 0), To: (n2, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n1, 1), To: (n3, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n2, 1), To: (n4, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n2, 2), To: (n5, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n5, 1), To: (n6, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n3, 1), To: (n7, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n4, 1), To: (n8, 0)));
            _formulaContext.ConnectNodeUseCase.Execute(new(From: (n5, 2), To: (n9, 0)));

            //DFS 124563
            //BFS 123456


            Debug.Log(n1);
            Debug.Log(n2);
            Debug.Log(n3);
            Debug.Log(n4);
        }
    }
}