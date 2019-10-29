using UnityEngine;

namespace Player
{
    public class Animation : MonoBehaviour
    {
        private Animator animator;
        private Animator powerAnimator;
        private Animator currentWeaponAnimator;

        private SpriteRenderer spriteRenderer;
        private SpriteRenderer currentWeaponSpriteRenderer;

        private Movement thisCharacterMovement;
        private Attack thisCharacterAttack;

        public GameObject runEffect;

        public bool attackAnimationPlaying = false;
        public bool powerAnimationPlaying = false;

        public bool overChargedAnimationPlaying = false;

        void Start()
        {
            animator = this.GetComponent<Animator>();
            spriteRenderer = this.GetComponent<SpriteRenderer>();

            thisCharacterMovement = this.GetComponent<Movement>();
            thisCharacterAttack = this.GetComponent<Attack>();

            currentWeaponAnimator = thisCharacterAttack.currentWeapon.GetComponent<Animator>();
            currentWeaponSpriteRenderer = thisCharacterAttack.currentWeapon.GetComponent<SpriteRenderer>();
            powerAnimator = thisCharacterAttack.currentPower.GetComponent<Animator>();
        }

        void Update()
        {
            this.DetermineRunState();
            this.DetermineMovementAndDirectionalState();
            this.DetermineAttackState();
            this.DetermineOverChargeState();
        }

        private void DetermineRunState()
        {
            ParticleSystemRenderer dustRenderer = null;

            if (thisCharacterMovement.isRunning)
            {
                animator.speed = 1.5f;

                // only emit if you're grounded and running
                if (animator.GetFloat("Speed") > 0.01)
                {
                    // create run effect
                    if (thisCharacterMovement.directionY < 0)
                    {
                        // we are facing down, so make change the order layer of the smoke
                        dustRenderer = runEffect.GetComponent<ParticleSystem>()?.GetComponent<ParticleSystemRenderer>();
                        dustRenderer.sortingOrder = 4;
                    }
                    else
                    {
                        // otherwise back to 10
                        dustRenderer = runEffect.GetComponent<ParticleSystem>()?.GetComponent<ParticleSystemRenderer>();
                        dustRenderer.sortingOrder = 6;
                    }

                    Instantiate(runEffect, new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.min.y), runEffect.transform.rotation);
                }
            }
            else
            {

                // character isn't running, reset animation speed
                animator.speed = 1.0f;
            }
        }

        private void DetermineMovementAndDirectionalState()
        {
            if (animator.GetFloat("Horizontal") != thisCharacterMovement.directionX)
            {
                // make sure to flip sprite if moving left or right
                if (thisCharacterMovement.directionX <= 0)
                {
                    spriteRenderer.flipX = true;

                    // TODO: shouldn't have to call attack from animator
                    thisCharacterAttack.FlipWeaponCollider(180.0f);
                }
                else
                {
                    spriteRenderer.flipX = false;

                    // TODO: shouldn't have to call attack from animator
                    thisCharacterAttack.FlipWeaponCollider(0.0f);
                }
            }

            if (animator.GetFloat("Vertical") != thisCharacterMovement.directionY)
            {
                // make sure to change sort order if moving up or down
                if (thisCharacterMovement.directionY <= 0)
                {
                    currentWeaponSpriteRenderer.sortingOrder = 6;
                }
                else
                {
                    currentWeaponSpriteRenderer.sortingOrder = 4;
                }
            }

            // set animation values to their cooresponding directions
            animator.SetFloat("Horizontal", thisCharacterMovement.directionX);
            animator.SetFloat("Vertical", thisCharacterMovement.directionY);

            // sqrMagnitude will always be positive when we are moving
            animator.SetFloat("Speed", thisCharacterMovement.currentMovement.sqrMagnitude);
        }

        private void DetermineAttackState()
        {
            // determine input triggers
            if (thisCharacterAttack.HasAttackBeenTriggered() && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !powerAnimator.GetBool("Charging"))
            {
                if (thisCharacterAttack.attackTriggered)
                {
                    // attack
                    currentWeaponAnimator.SetTrigger("Attack");
                    attackAnimationPlaying = true;
                }
                else if (thisCharacterAttack.powerCharging)
                {
                    // power
                    powerAnimator.SetBool("Charging", true);
                    animator.SetBool("Is Carrying", true);
                    powerAnimationPlaying = true;
                }
            }

            this.ResetAttackAnimations();
        }

        private void DetermineOverChargeState()
        {
            if (thisCharacterAttack.powerCharging)
            {
                if (powerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Over Charged"))
                {
                    this.RenderCharacterOverCharged();
                }
            }
            else
            {
                if (powerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Over Charged"))
                {
                    this.ResetCharacter();
                    powerAnimator.Play("No Power");
                }
            }
        }

        private void RenderCharacterOverCharged()
        {
            spriteRenderer.color = Color.black;
        }

        private void ResetCharacter()
        {
            spriteRenderer.color = Color.white;
        }

        private void ResetAttackAnimations()
        {
            if (!thisCharacterAttack.HasAttackBeenTriggered())
            {
                if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    // attack
                    attackAnimationPlaying = false;
                }

                if (powerAnimator.GetBool("Charging"))
                {
                    // power
                    powerAnimationPlaying = false;
                    powerAnimator.SetBool("Charging", false);
                    animator.SetBool("Is Carrying", false);
                }
            }
        }

    }
}
