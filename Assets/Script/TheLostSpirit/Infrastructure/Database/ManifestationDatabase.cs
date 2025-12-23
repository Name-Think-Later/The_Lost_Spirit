using TheLostSpirit.Identity.SpecificationID;
using TheLostSpirit.Infrastructure.Domain.Specification;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [CreateAssetMenu(fileName = "Manifestation Database",
                     menuName = "The Lost Spirits/DataBase/Manifestation DataBase")]
    public class ManifestationDatabase
        : GenericDatabase<ManifestationSpecificationID, ManifestationSpecification>
    { }
}