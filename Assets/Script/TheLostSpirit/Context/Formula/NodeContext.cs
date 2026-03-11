using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Formula;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Repository;

namespace TheLostSpirit.Context.Formula
{
    [Serializable, FoldoutGroup("Node Context"), HideLabel]
    public class NodeContext
    {
        public NodeRepository NodeRepository { get; private set; }

        public CreateNodeUseCase CreateNodeUseCase { get; private set; }
        public ConnectNodeUseCase ConnectNodeUseCase { get; private set; }
        public NodeContainSkillUseCase NodeContainSkillUseCase { get; private set; }

        public NodeContext Construct(SkillContext skillContext) {
            NodeRepository = new NodeRepository();

            CreateNodeUseCase       = new CreateNodeUseCase(NodeRepository);
            ConnectNodeUseCase      = new ConnectNodeUseCase(NodeRepository);
            NodeContainSkillUseCase = new NodeContainSkillUseCase(NodeRepository);

            _ = new AsyncVisitedNodeEventHandler(
                NodeRepository,
                skillContext.AnchorRepository,
                skillContext.ActiveSkillUseCase
            );

            return this;
        }
    }
}