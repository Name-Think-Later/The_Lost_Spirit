using System;
using UnityEngine;

namespace TheLostSpirit.Domain.Player
{
    [Serializable]
    public class PlayerConfig
    {
        [SerializeField]
        float baseSpeed = 10;

        [SerializeField]
        float baseJumpForce = 10;

        [SerializeField]
        float jumpingGravityScale = 0.33f;

        public float BaseSpeed => baseSpeed;

        public float BaseJumpForce => baseJumpForce;

        public float JumpingGravityScale => jumpingGravityScale;
    }
}