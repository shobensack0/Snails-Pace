using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        private Movement thisCharacterMovement;
        private Animation thisCharacterAnimation;

        private PolygonCollider2D currentWeaponCollider;

        public GameObject currentWeapon;
        public GameObject currentPower;

        public bool attackTriggered = false;
        public bool powerCharging = false;

        // Start is called before the first frame update
        void Start()
        {
            thisCharacterMovement = this.GetComponent<Movement>();
            thisCharacterAnimation = this.GetComponent<Animation>();

            if (currentWeapon != null)
            {
                currentWeapon.TryGetComponent(out currentWeaponCollider);
            }
        }

        void Update()
        {
            var attackInputOnePressed = Input.GetAxisRaw("Fire1") > 0.0f;
            var attackInputTwoPressed = Input.GetAxisRaw("Fire2") > 0.0f;

            this.DetermineAttackState(attackInputOnePressed);
            this.DeterminePowerState(attackInputTwoPressed);
        }

        public bool HasAttackBeenTriggered()
        {
            return attackTriggered || powerCharging;
        }

        public void FlipWeaponCollider(float flipDegrees)
        {
            // TODO: not my favorite way to do this, we should be able to determine direction from here not outside (ie right now its in the animator
            currentWeaponCollider.transform.localEulerAngles = new Vector3(0f, flipDegrees, 0f);
        }

        private void DetermineAttackState(bool attackInputOnePressed)
        {
            if (!HasAttackBeenTriggered() && attackInputOnePressed)
            {
                // no trigger set and input pressed, trigger attack
                attackTriggered = true;
            }

            this.ResetAttackTrigger();
        }

        private void DeterminePowerState(bool inputPressed)
        {
            if (!HasAttackBeenTriggered() && inputPressed)
            {
                // no trigger set and input pressed, start charging
                powerCharging = true;
            }

            this.ResetPowerCharging(inputPressed);
        }

        private void ResetAttackTrigger()
        {
            // attack
            if (thisCharacterAnimation.attackAnimationPlaying)
            {
                // animation is playing, we can reset trigger
                attackTriggered = false;
            }
        }

        private void ResetPowerCharging(bool inputPressed)
        {
            // power
            if (!inputPressed)
            {
                powerCharging = false;
            }
        }
    }
}
