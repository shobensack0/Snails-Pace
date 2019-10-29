using UnityEngine;

namespace Player
{
    public class Animation : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private Movement characterMovement;
        private Attack characterAttack;

        public GameObject runEffect;

        public bool attackAnimationPlaying = false;
        public bool powerAnimationPlaying = false;

        private Animator currentWeaponAnimator;
        private SpriteRenderer currentWeaponSpriteRenderer;
        private Animator powerAnimator;


        // Start is called before the first frame update
        void Start()
        {
            animator = this.GetComponent<Animator>();
            spriteRenderer = this.GetComponent<SpriteRenderer>();

            characterMovement = this.GetComponent<Movement>();
            characterAttack = this.GetComponent<Attack>();

            currentWeaponAnimator = characterAttack.currentWeapon.GetComponent<Animator>();
            currentWeaponSpriteRenderer = characterAttack.currentWeapon.GetComponent<SpriteRenderer>();
            powerAnimator = characterAttack.currentPower.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            this.DetermineRunState();
            this.DetermineMovementState();
            this.DetermineAttackState();
        }

        private void DetermineRunState()
        {
            ParticleSystemRenderer dustRenderer = null;

            if (characterMovement.isRunning)
            {
                animator.speed = 1.5f;

                // only emit if you're grounded and running
                if (animator.GetFloat("Speed") > 0.01)
                {
                    // create run effect
                    if (characterMovement.directionY < 0)
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

        private void DetermineMovementState()
        {
            if (animator.GetFloat("Horizontal") != characterMovement.directionX)
            {
                // make sure to flip sprite if moving left or right
                if (characterMovement.directionX <= 0)
                {
                    spriteRenderer.flipX = true;

                    // weapon stuff TODO: get this the hell out of here
                    //currentWeaponCollider.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                }
                else
                {
                    spriteRenderer.flipX = false;

                    // weapon stuff TODO: get this the hell out of here
                    //currentWeaponCollider.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                }
            }

            if (animator.GetFloat("Vertical") != characterMovement.directionY)
            {
                // make sure to change sort order if moving up or down
                if (characterMovement.directionY <= 0)
                {
                    //currentWeaponSpriteRenderer.sortingOrder = 6;
                }
                else
                {
                    //currentWeaponSpriteRenderer.sortingOrder = 4;
                }

            }

            // set animation values to their cooresponding directions
            animator.SetFloat("Horizontal", characterMovement.directionX);
            animator.SetFloat("Vertical", characterMovement.directionY);

            // sqrMagnitude will always be positive when we are moving
            animator.SetFloat("Speed", characterMovement.currentMovement.sqrMagnitude);
        }

        private void DetermineAttackState()
        {
            if (characterAttack.isUsingAttack && !characterAttack.isUsingPower)
            {
                if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !powerAnimator.GetBool("Charging"))
                {
                    attackAnimationPlaying = true;
                    currentWeaponAnimator.SetTrigger("Attack");
                }
            }
            else if (characterAttack.isUsingPower && !characterAttack.isUsingAttack)
            {
                if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !powerAnimator.GetBool("Charging"))
                {
                    powerAnimationPlaying = true;
                    powerAnimator.SetBool("Charging", true);
                    animator.SetBool("Is Carrying", true);
                }
            }
            else
            {
                // not attacking, but see if animations are playing
                if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                }
            }
        }
    }
}
