using UnityEngine;

namespace Weapons
{
    public class IronHammer : WeaponMonoBehavior
    {
        public void Start()
        {
            if (staminaUsage == 0.0f)
            {
                staminaUsage = 50.0f;
            }

            if (damageRange == null || damageRange.max == 0.0f)
            {
                damageRange = new DamageRange(30.0f, 45.0f);
            }
        }

        public override void DetermineWeaponDirection()
        {
            if (character_Animator.GetFloat("Horizontal") != character_Movement.directionX)
            {
                if (character_Movement.directionX <= 0)
                    this_Collider.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                else
                    this_Collider.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }

            // TODO: hammer needs to be "behind" character for first half, "in front" the second half of animation
            //anim.GetCurrentAnimatorStateInfo(0).IsName("Attack").normalizedTime == 0.5f;
            if (character_Animator.GetFloat("Vertical") != character_Movement.directionY)
            {
                if (character_Movement.directionY <= 0)
                    this_SpriteRenderer.sortingOrder = 6;
                else
                    this_SpriteRenderer.sortingOrder = 4;
            }
        }
    }
}