using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Component;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Extension.General;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;
using Object = UnityEngine.Object;

public class DummyMono
    : MonoBehaviour,
      IEntityMono, IDamageableComponent, IForceApplyingComponent
{
    [SerializeField]
    Rigidbody2D _rigidbody;

    public IRuntimeID ID { get; }
    public IReadOnlyTransform ReadOnlyTransform { get; }

    public void Destroy() {
        Object.Destroy(gameObject);
    }

    public void DealDamage(float amount) {
        Debug.Log($"{name} Got Damage: {amount}".Colored(new Color(0.8f, 0.2f, 0.2f)));
    }

    public void ApplyForce(Vector2 force) {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}