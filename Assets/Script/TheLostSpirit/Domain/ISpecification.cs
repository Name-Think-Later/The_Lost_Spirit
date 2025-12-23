using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain
{
    public interface ISpecification<out TSpecificationID> where TSpecificationID : ISpecificationID
    {
        TSpecificationID ID { get; }
    }
}