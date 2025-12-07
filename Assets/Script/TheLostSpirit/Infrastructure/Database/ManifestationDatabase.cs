using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Infrastructure.Domain.ConfigWrapper;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [CreateAssetMenu(fileName = "Manifestation Database",
                     menuName = "The Lost Spirits/DataBase/Manifestation DataBase")]
    public class ManifestationDatabase
        : GenericDatabase<ManifestationConfigID, ManifestationConfig, ManifestationConfigWrapper>
    { }
}