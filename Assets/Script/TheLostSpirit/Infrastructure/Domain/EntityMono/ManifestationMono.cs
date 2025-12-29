using System.Linq;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheLostSpirit.Infrastructure.Domain.EntityMono
{
    public class ManifestationMono : MonoBehaviour, IManifestationMono
    {
        FrameActions   _frameActions;
        FormulaPayload _payload;

        public ManifestationID ID { get; private set; }
        IRuntimeID IEntityMono.ID => ID;
        public IReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Initialize(ManifestationID id) {
            ID                = id;
            ReadOnlyTransform = transform.ToReadOnly();
        }

        public void Initialize(
            ManifestationID id,
            FrameActions    frameActions,
            FormulaPayload  payload
        ) {
            this.Initialize(id);
            _frameActions = frameActions;
            _payload      = payload;
            foreach (var action in frameActions.Values) {
                foreach (var combatStep in action) {
                    combatStep.SetOwner(transform);
                }
            }
        }

        public void DoFrameActions(int index) {
            foreach (var combatStep in _frameActions[index]) {
                combatStep.Do(_payload).Forget();
            }
        }


        public void Destroy() {
            Object.Destroy(gameObject);
        }
    }
}