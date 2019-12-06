using UnityEngine;

namespace Weapons
{
    public class DamageRange
    {
        public int min;
        public int max;

        public DamageRange(int max)
        {
            min = 0;
            this.max = max;
        }

        public DamageRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int GetDamageAmount()
        {
            return Random.Range(min, max);
        }
    }
}
