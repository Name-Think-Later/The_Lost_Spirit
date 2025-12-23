using System.Linq;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class ManifestationMono : MonoBehaviour, IManifestationMono
    {
        FrameActions _frameActions;

        public ManifestationID ID { get; private set; }
        IRuntimeID IEntityMono.ID => ID;
        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(ManifestationID id) {
            ID                = id;
            ReadOnlyTransform = transform.ToReadOnly();
        }

        public void Initialize(
            ManifestationID id,
            FrameActions    frameActions
        ) {
            this.Initialize(id);
            _frameActions = frameActions;

            var combatSteps = _frameActions.Values.SelectMany(steps => steps);
            foreach (var combatStep in combatSteps) {
                combatStep.Initialize(transform);
            }
        }

        public void DoFrameActions(int index) {
            foreach (var combatStep in _frameActions[index]) {
                combatStep.Do().Forget();
            }
        }


        public void Destroy() {
            Object.Destroy(gameObject);
        }
    }
}