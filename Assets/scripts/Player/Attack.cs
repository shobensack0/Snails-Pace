using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        private Movement characterMovement;
        private Animation characterAnimation;

        public GameObject currentWeapon;
        private PolygonCollider2D currentWeaponCollider;

        public GameObject currentPower;

        public bool isUsingAttack = false;
        public bool isUsingPower = false;

        // Start is called before the first frame update
        void Start()
        {
            characterMovement = this.GetComponent<Movement>();
            characterAnimation = this.GetComponent<Animation>();


            // TODO: this NEEDS to be abstracted, AND REMOVE
            if (currentWeapon != null)
            {
                currentWeapon.TryGetComponent(out currentWeaponCollider);
            }
        }

        // Update is called once per frame
        void Update()
        {
            var attackInputOnePressed = Input.GetAxisRaw("Fire1") > 0.0f;
            var attackInputTwoPressed = Input.GetAxisRaw("Fire2") > 0.0f;

            this.DetermineAttackState(attackInputOnePressed, attackInputTwoPressed);
        }

        private void DetermineAttackState(bool attackInputOnePressed, bool attackInputTwoPressed)
        {
            if (attackInputOnePressed)
            {
                // attack
                if (!characterAnimation.attackAnimationPlaying && !characterAnimation.powerAnimationPlaying)
                {
                    isUsingAttack = true;
                }
            }
            else if (attackInputTwoPressed)
            {
                // power
                if (!characterAnimation.attackAnimationPlaying && !characterAnimation.powerAnimationPlaying)
                {
                    isUsingPower = true;
                }
            }

            // check animations to turn off current action
            if (!characterAnimation.attackAnimationPlaying && !attackInputOnePressed)
            {
                isUsingAttack = false;
            }
            else if (!characterAnimation.powerAnimationPlaying && !attackInputTwoPressed)
            {
                isUsingPower = false;
            }

        }
    }
}
