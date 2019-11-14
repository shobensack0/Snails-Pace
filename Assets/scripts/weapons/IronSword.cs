using UnityEngine;

namespace Weapons
{
    public class IronSword : WeaponMonoBehavior
    {
        public void Start()
        {
            if (staminaUsage == 0.0f)
            {
                staminaUsage = 25.0f;
            }

            if (damageRange == null || damageRange.max == 0.0f)
            {
                damageRange = new DamageRange(10.0f, 20.0f);
            }
        }
    }
}