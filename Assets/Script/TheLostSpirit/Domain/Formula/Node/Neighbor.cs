using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula.Node
{
    public record struct Neighbor(NodeID ID, AssociateType Type)
    {
        public bool IsIn => Type == AssociateType.In;
        public bool IsOut => Type == AssociateType.Out;
    }
}