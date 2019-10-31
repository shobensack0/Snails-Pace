using UnityEngine;

namespace Player
{
    public class Movement : PlayerScriptMonoBehavior
    {
        #region Effects 
        public GameObject runEffect;
        #endregion

        #region Public Variables 
        public bool isRunning = false;
        public bool isMoving = false;

        public float directionX = 0.0f;
        public float directionY = -1.0f;
        #endregion

        #region Movement Variables
        private Vector2 currentMovement;
        private float currentMoveSpeed = 5f;

        private readonly float minRunSpeed = 5.0f;
        private readonly float maxRunSpeed = 10f;
        private readonly float runAcceleration = 10f;
        private readonly float runDeceleration = 12f;
        #endregion

        public void Start()
        {
            this.SetCharacterComponents();
        }

        public void FixedUpdate()
        {
            // TODO: normalized is bad for controllers
            character_RigidBody.MovePosition(character_RigidBody.position + currentMovement.normalized * currentMoveSpeed * Time.fixedDeltaTime);
        }

        public void Update()
        {
            var horizontalMovement = Input.GetAxisRaw("Horizontal");
            var verticalMovement = Input.GetAxisRaw("Vertical");
            var runInput = Input.GetAxisRaw("Fire3") > 0.0f;

            this.DetermineMovementAndDirectionalState();
            this.DetermineRunState(runInput);

            this.SetDirection(horizontalMovement, verticalMovement, ref directionX, ref directionY);
            this.SetDirection(verticalMovement, horizontalMovement, ref directionY, ref directionX);
            this.currentMovement = new Vector2(horizontalMovement, verticalMovement);

            this.isMoving = horizontalMovement > 0 || verticalMovement > 0;
        }

        #region Private Methods
        private void SetDirection(float thisInputMovement, float otherInputMovement, ref float thisDirection, ref float otherDirection)
        {
            if (thisInputMovement != 0)
            {
                // input has changed
                thisDirection = thisInputMovement;

                // check other input for changes
                if (otherInputMovement != 0)
                {
                    otherDirection = otherInputMovement;
                }
            }
        }

        private void DetermineRunState(bool inputActive)
        {
            if (inputActive)
            {
                isRunning = true;
                character_Animator.speed = 1.5f;

                // determine acceleration of speed
                if (currentMoveSpeed < maxRunSpeed)
                    currentMoveSpeed += runAcceleration * Time.deltaTime;
                else
                    currentMoveSpeed = maxRunSpeed;

                // only emit if you're grounded and running
                if (character_Animator.GetFloat("Speed") > 0.01)
                {
                    ParticleSystemRenderer dustRenderer = null;

                    // create run effect
                    if (directionY < 0)
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

                    Instantiate(runEffect, new Vector2(character_SpriteRenderer.bounds.center.x, character_SpriteRenderer.bounds.min.y), runEffect.transform.rotation);
                }
            }
            else
            {
                isRunning = false;
                character_Animator.speed = 1.0f;

                // determine deceleration
                if (currentMoveSpeed > minRunSpeed)
                    currentMoveSpeed -= runDeceleration * Time.deltaTime;
                else
                    currentMoveSpeed = minRunSpeed;
            }
        }

        private void DetermineMovementAndDirectionalState()
        {
            if (character_Animator.GetFloat("Horizontal") != directionX)
            {
                if (directionX <= 0)
                    character_SpriteRenderer.flipX = true;
                else
                    character_SpriteRenderer.flipX = false;
            }

            // set animation values to their cooresponding directions
            character_Animator.SetFloat("Horizontal", directionX);
            character_Animator.SetFloat("Vertical", directionY);

            // sqrMagnitude will always be positive when we are moving
            character_Animator.SetFloat("Speed", currentMovement.sqrMagnitude);
        }
        #endregion
    }
}

