using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.ConfigID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Event
{
    [Serializable]
    public class ManifestationConfig : IConfig<ManifestationConfigID>
    {
        [SerializeField, HideLabel]
        ManifestationConfigID _id;

        public ManifestationConfigID ID => _id;
    }
}