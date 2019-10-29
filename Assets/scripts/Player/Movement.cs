using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        private Rigidbody2D rigidBody;

        public Vector2 currentMovement;
        public float currentMoveSpeed = 5f;
        public float directionX = 0.0f;
        public float directionY = -1.0f;

        public readonly float minRunSpeed = 5.0f;
        public readonly float maxRunSpeed = 10f;
        public readonly float runAcceleration = 10f;
        public readonly float runDeceleration = 12f;

        public bool isRunning = false;
        public bool isMoving = false;

        void Start()
        {
            rigidBody = this.GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            // movement here, updates 50 times per second
            // TODO: normalized is bad for controllers
            rigidBody.MovePosition(rigidBody.position + currentMovement.normalized * currentMoveSpeed * Time.fixedDeltaTime);
        }

        void Update()
        {
            var horizontalMovement = Input.GetAxisRaw("Horizontal");
            var verticalMovement = Input.GetAxisRaw("Vertical");
            var runInput = Input.GetAxisRaw("Fire3") > 0.0f;

            this.SetRunState(runInput);

            this.SetDirection(horizontalMovement, verticalMovement, ref directionX, ref directionY);
            this.SetDirection(verticalMovement, horizontalMovement, ref directionY, ref directionX);
            currentMovement = new Vector2(horizontalMovement, verticalMovement);

            this.isMoving = horizontalMovement > 0 || verticalMovement > 0;
        }

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

        private void SetRunState(bool isRunning)
        {
            // set running state
            if (isRunning)
            {
                this.isRunning = true;

                // determine acceleration of speed
                if (currentMoveSpeed < maxRunSpeed)
                {
                    currentMoveSpeed += runAcceleration * Time.deltaTime;
                }
                else
                {
                    currentMoveSpeed = maxRunSpeed;
                }
            }
            else
            {
                this.isRunning = false;

                // determine deceleration
                if (currentMoveSpeed > minRunSpeed)
                {
                    currentMoveSpeed -= runDeceleration * Time.deltaTime;
                }
                else
                {
                    currentMoveSpeed = minRunSpeed;
                }
            }
        }
    }
}

