using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Extension.General;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;
using Object = UnityEngine.Object;

public class DummyMono : MonoBehaviour, IEntityMono, IDamageableComponent
{
    public IRuntimeID ID { get; }
    public IReadOnlyTransform ReadOnlyTransform { get; }

    public void Destroy() {
        Object.Destroy(gameObject);
    }

    public void DealDamage(float amount) {
        Debug.Log($"{name} Got Damage: {amount}".Colored(new Color(0.8f, 0.2f, 0.2f)));
    }
}