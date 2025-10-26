using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Node {
    public record struct Neighbor(NodeID ID, AssociateType Type);
}