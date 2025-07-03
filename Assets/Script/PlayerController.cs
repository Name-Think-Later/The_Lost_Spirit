using Script;
using UnityEngine;

public class PlayerController {
    readonly PlayerReference _reference;

    float _axis = 0;

    public PlayerController(
        PlayerReference reference
    ) {
        _reference = reference;
    }

    public void SetAxis(float axis) {
        _axis = axis;
    }

    public void ApplyVelocity() {
        var rigidbody = _reference.Rigidbody;
        var velocity  = rigidbody.velocity.WithX(_axis * _reference.Speed);
        _reference.Rigidbody.velocity = velocity;
    }
}