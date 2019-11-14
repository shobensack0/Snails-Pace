using UnityEngine;

namespace Weapons
{
    public class DamageRange
    {
        public float min;
        public float max;

        public DamageRange(float max)
        {
            min = 0;
            this.max = max;
        }

        public DamageRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetDamageAmount()
        {
            return Mathf.Ceil(Random.Range(min, max));
        }
    }
}
