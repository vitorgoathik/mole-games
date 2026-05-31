using UnityEngine;

namespace Platformer.Evolution
{
    [System.Serializable]
    public class FormTraits
    {
        [Range(1f, 20f)] public float maxSpeed = 7f;
        [Range(1f, 20f)] public float jumpTakeOffSpeed = 7f;
        [Range(0.5f, 3f)] public float jumpModifier = 1.5f;
        public bool canDoubleJump = false;
        public bool canDash = false;
        [Range(0f, 10f)] public float dashSpeed = 0f;
        [Range(0f, 5f)] public float dashCooldown = 0f;
    }
}
