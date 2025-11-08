using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Node
{
    public record struct Neighbor(NodeID ID, AssociateType Type)
    {
        public bool IsIn => Type == AssociateType.In;
        public bool IsOut => Type == AssociateType.Out;
    };
}