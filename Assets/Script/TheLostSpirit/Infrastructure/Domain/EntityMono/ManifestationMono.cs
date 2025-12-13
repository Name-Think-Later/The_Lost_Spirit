using System.Collections.Generic;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class ManifestationMono : MonoBehaviour, IManifestationMono
    {
        public ManifestationID ID { get; private set; }
        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(ManifestationID id) {
            ID = id;

            ReadOnlyTransform = transform.ToReadOnly();
        }

        public void Destroy() {
            Object.Destroy(gameObject);
        }
    }
}