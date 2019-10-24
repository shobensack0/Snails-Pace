using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isDebug = false;

    // config values
    private float defaultMoveSpeed = 5f;

    // game components
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // effects
    public GameObject runEffect;

    // weapons
    public GameObject currentWeapon;
    private Animator currentWeaponAnimator;
    private SpriteRenderer currentWeaponSpriteRenderer;
    private PolygonCollider2D currentWeaponCollider;

    private bool isAttacking = false;

    // setable components
    private Vector2 movement;
    private float directionX = 0.0f;
    private float directionY = -1.0f;
    private float jumpTimer = 0.0f;

    private float currentMoveSpeed = 5f;
    private readonly float attackMoveSpeed = 1f;
    private readonly float carryMoveSpeed = 2f;
    private readonly float minRunSpeed = 5.0f;
    private readonly float maxRunSpeed = 10f;
    private readonly float runAcceleration = 10f;
    private readonly float runDeceleration = 12f;

    private readonly float JUMP_TIME = 0.5f;

    // DEBUG
    private float directionDebugCastDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        // TODO: this NEEDS to be abstracted
        if (currentWeapon != null)
        {
            currentWeapon.TryGetComponent(out currentWeaponAnimator);
            currentWeapon.TryGetComponent(out currentWeaponSpriteRenderer);
            currentWeapon.TryGetComponent(out currentWeaponCollider);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.SetMovementInput();
        this.SetAnimation();

        // DEBUG STUFF
        this.ConsoleDebug(isDebug);
    }

    private void FixedUpdate()
    {
        // movement here, updates 50 times per second
        // TODO: normalized is bad for controllers
        rigidBody.MovePosition(rigidBody.position + movement.normalized * currentMoveSpeed * Time.fixedDeltaTime);
    }

    private void SetMovementInput()
    {
        // get inputs
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var isMoving = animator.GetFloat("Speed") > 0.0f;
        var isRunning = Input.GetAxisRaw("Fire3") > 0.0f;
        var jumpPressed = Input.GetAxisRaw("Jump") > 0.0f;
        var attackOneInput = Input.GetAxisRaw("Fire1") > 0.0f;
        var attackTwoInput = Input.GetAxisRaw("Fire2") > 0.0f;

        // set direction in x and y
        this.SetDirection(horizontalMovement, verticalMovement, ref directionX, ref directionY);
        this.SetDirection(verticalMovement, horizontalMovement, ref directionY, ref directionX);

        // set motion states
        this.SetRunState(isRunning);
        this.SetJumpState(jumpPressed);
        this.SetAttackState(attackOneInput, attackTwoInput, isMoving);

        // set movement
        movement = new Vector2(horizontalMovement, verticalMovement);
    }

    private void SetAttackState(bool attackOneTriggered, bool attackTwoTriggered, bool isMoving)
    {
        if (isAttacking)
        {
            if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                // not attacking anymore, so reset
                isAttacking = false;

                // just make sure weapon collider is definitely disabled
                currentWeaponCollider.enabled = false;
                currentMoveSpeed = minRunSpeed;
            }
        }
        else
        {
            // were not attacking, so okay to trigger another
            if (attackOneTriggered && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                currentWeaponAnimator.SetTrigger("Attack");
                isAttacking = true;
                currentMoveSpeed = attackMoveSpeed;
            }
            else if (attackTwoTriggered)
            {
                animator.SetBool("Is Carrying", true);
                currentMoveSpeed = carryMoveSpeed;
                animator.speed = 0.5f;
            }
            else
            {
                animator.SetBool("Is Carrying", false);
                currentMoveSpeed = minRunSpeed;
                animator.speed = 1.0f;
            }
        }
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
            ParticleSystemRenderer dustRenderer = null;

            // determine acceleration of speed
            if (currentMoveSpeed < maxRunSpeed)
            {
                currentMoveSpeed += runAcceleration * Time.deltaTime;
            }
            else
            {
                currentMoveSpeed = maxRunSpeed;
            }

            animator.speed = 1.5f;

            // only emit if you're grounded and running
            if (animator.GetBool("Is Grounded") && animator.GetFloat("Speed") > 0.01)
            {
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

                Instantiate(runEffect, new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.min.y), runEffect.transform.rotation);
            }
        }
        else
        {
            // determine deceleration
            if (currentMoveSpeed > minRunSpeed)
            {
                currentMoveSpeed -= runDeceleration * Time.deltaTime;
            }
            else
            {
                currentMoveSpeed = minRunSpeed;
            }

            animator.speed = 1.0f;

        }
    }

    private void SetJumpState(bool jumpPressed)
    {
        if (jumpPressed)
        {
            // grounded
            if (animator.GetBool("Is Grounded"))
            {
                animator.SetBool("Is Grounded", false);
            } 
        }

        if (!animator.GetBool("Is Grounded"))
        {
            if (jumpTimer <= JUMP_TIME)
            {
                jumpTimer += Time.deltaTime;
            }

            if (jumpTimer >= JUMP_TIME)
            {
                jumpTimer = 0;
                animator.SetBool("Is Grounded", true);
            }
        }
    }

    private void SetAnimation()
    {
        if (animator.GetFloat("Horizontal") != directionX) {
            // make sure to flip sprite if moving left or right
            if (this.directionX <= 0)
            {
                spriteRenderer.flipX = true;

                // weapon stuff TODO: get this the hell out of here
                currentWeaponCollider.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                spriteRenderer.flipX = false;

                // weapon stuff TODO: get this the hell out of here
                currentWeaponCollider.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        if (animator.GetFloat("Vertical") != directionY) {
            // make sure to change sort order if moving up or down
            if (this.directionY <= 0)
            {
                currentWeaponSpriteRenderer.sortingOrder = 6;
            }
            else
            {
                currentWeaponSpriteRenderer.sortingOrder = 4;
            }

        }

        // set animation values to their cooresponding directions
        animator.SetFloat("Horizontal", directionX);
        animator.SetFloat("Vertical", directionY);

        // sqrMagnitude will always be positive when we are moving
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void ConsoleDebug(bool isDebug = false)
    {
        // yoooooo use this to determine space infront of character, THIS IS SOME GOOD SHIT RIGHT HUR
        // right now the normalizer value makes diagaonal rays shorter
        var normalizer = (directionX != 0 && directionY != 0) ? 2 : 1;
        var endRayCastX = transform.position.x + ((directionDebugCastDistance * directionX) / normalizer);
        var endRayCastY = transform.position.y + ((directionDebugCastDistance * directionY) / normalizer);
        var endRaycast = new Vector2(endRayCastX, endRayCastY);

        //Debug.DrawLine(transform.position, endRaycast, Color.magenta);
    }
}

// neat code but not uesed

//private void setAttackState(bool isAttacking, bool isMoving)
//{
//    if (isAttacking)
//    {
//        animator.SetBool("Is Attacking", true);

//        // TODO: determine diagonal

//        // determine start and end attack spread

//        if (directionX > 0)
//        {
//            // attacking right

//            // TODO: this can all be abstracted don't be dumb with your math
//            var startAttackPointX = transform.position.x + attackSpread;
//            var startAttackPointY = transform.position.y - attackSpread;
//            var endAttackPointX = transform.position.x + attackSpread;
//            var endAttackPointY = transform.position.y + attackSpread;

//            var startAttackPoint = new Vector2(startAttackPointX, startAttackPointY);
//            var endAttackPoint = new Vector2(endAttackPointX, endAttackPointY);

//            var movementOffset = 0.0f;

//            if (isMoving)
//            {
//                movementOffset = movement.normalized.x * currentMoveSpeed * Time.deltaTime;
//            }

//            // attacking down, swinging in positive direction on X axis
//            Debug.DrawLine(transform.position, startAttackPoint, Color.magenta);
//            Debug.DrawLine(transform.position, endAttackPoint, Color.red);

//            if (currentAttackPoint == Vector2.zero)
//            {
//                // start the attack
//                currentAttackPoint = startAttackPoint;
//            }

//            if (currentAttackPoint.y < endAttackPoint.y)
//            {
//                // keep moving to the right
//                currentAttackPoint.x += movementOffset;
//                currentAttackPoint.y += attackSpeed * Time.deltaTime;
//                Debug.DrawLine(transform.position, currentAttackPoint, Color.cyan);
//            }

//            if (currentAttackPoint.y >= endAttackPoint.y)
//            {
//                // attack over, reset
//                currentAttackPoint = Vector2.zero;
//                animator.SetBool("Is Attacking", false);
//            }
//        }
//        else if (directionX < 0)
//        {
//            // attacking left

//            // TODO: this can all be abstracted don't be dumb with your math
//            var startAttackPointX = transform.position.x - attackSpread;
//            var startAttackPointY = transform.position.y + attackSpread;
//            var endAttackPointX = transform.position.x - attackSpread;
//            var endAttackPointY = transform.position.y - attackSpread;

//            var startAttackPoint = new Vector2(startAttackPointX, startAttackPointY);
//            var endAttackPoint = new Vector2(endAttackPointX, endAttackPointY);

//            var movementOffset = 0.0f;

//            if (isMoving)
//            {
//                movementOffset = movement.normalized.x * currentMoveSpeed * Time.deltaTime;
//            }

//            // attacking down, swinging in positive direction on X axis
//            Debug.DrawLine(transform.position, startAttackPoint, Color.magenta);
//            Debug.DrawLine(transform.position, endAttackPoint, Color.red);

//            if (currentAttackPoint == Vector2.zero)
//            {
//                // start the attack
//                currentAttackPoint = startAttackPoint;
//            }

//            if (currentAttackPoint.y > endAttackPoint.y)
//            {
//                // keep moving to the right
//                currentAttackPoint.x += movementOffset;
//                currentAttackPoint.y -= attackSpeed * Time.deltaTime;
//                Debug.DrawLine(transform.position, currentAttackPoint, Color.cyan);
//            }

//            if (currentAttackPoint.y <= endAttackPoint.y)
//            {
//                // attack over, reset
//                currentAttackPoint = Vector2.zero;
//                animator.SetBool("Is Attacking", false);
//            }
//        }
//        else if (directionY > 0)
//        {
//            // attacking up

//            // TODO: this can all be abstracted don't be dumb with your math
//            var startAttackPointX = transform.position.x + attackSpread;
//            var startAttackPointY = transform.position.y + attackSpread;
//            var endAttackPointX = transform.position.x - attackSpread;
//            var endAttackPointY = transform.position.y + attackSpread;

//            var startAttackPoint = new Vector2(startAttackPointX, startAttackPointY);
//            var endAttackPoint = new Vector2(endAttackPointX, endAttackPointY);

//            var movementOffset = 0.0f;

//            if (isMoving)
//            {
//                movementOffset = movement.normalized.y * currentMoveSpeed * Time.deltaTime;
//            }

//            // attacking down, swinging in positive direction on X axis
//            Debug.DrawLine(transform.position, startAttackPoint, Color.magenta);
//            Debug.DrawLine(transform.position, endAttackPoint, Color.red);

//            if (currentAttackPoint == Vector2.zero)
//            {
//                // start the attack
//                currentAttackPoint = startAttackPoint;
//            }

//            if (currentAttackPoint.x > endAttackPoint.x)
//            {
//                // keep moving to the right
//                currentAttackPoint.x -= attackSpeed * Time.deltaTime;
//                currentAttackPoint.y += movementOffset;
//                Debug.DrawLine(transform.position, currentAttackPoint, Color.cyan);
//            }

//            if (currentAttackPoint.x <= endAttackPoint.x)
//            {
//                // attack over, reset
//                currentAttackPoint = Vector2.zero;
//                animator.SetBool("Is Attacking", false);
//            }
//        }
//        else if (directionY < 0)
//        {
//            // for arc:
//            // https://gamedev.stackexchange.com/questions/157642/moving-a-2d-object-along-circular-arc-between-two-points

//            // TODO: this can all be abstracted don't be dumb with your math
//            var startAttackPointX = transform.position.x - attackSpread;
//            var startAttackPointY = transform.position.y - attackSpread;
//            var endAttackPointX = transform.position.x + attackSpread;
//            var endAttackPointY = transform.position.y - attackSpread;

//            var startAttackPoint = new Vector2(startAttackPointX, startAttackPointY);
//            var endAttackPoint = new Vector2(endAttackPointX, endAttackPointY);

//            var movementOffset = 0.0f;

//            if (isMoving)
//            {
//                movementOffset = movement.normalized.y * currentMoveSpeed * Time.deltaTime;
//            }

//            // attacking down, swinging in positive direction on X axis
//            Debug.DrawLine(transform.position, startAttackPoint, Color.magenta);
//            Debug.DrawLine(transform.position, endAttackPoint, Color.red);

//            if (currentAttackPoint == Vector2.zero)
//            {
//                // start the attack
//                currentAttackPoint = startAttackPoint;
//            }

//            if (currentAttackPoint.x < endAttackPoint.x)
//            {
//                // keep moving to the right
//                currentAttackPoint.x += attackSpeed * Time.deltaTime;
//                currentAttackPoint.y += movementOffset;
//                Debug.DrawLine(transform.position, currentAttackPoint, Color.cyan);
//            }

//            if (currentAttackPoint.x >= endAttackPoint.x)
//            {
//                // attack over, reset
//                currentAttackPoint = Vector2.zero;
//                animator.SetBool("Is Attacking", false);
//            }
//        }
//    }
//    else
//    {
//        // reset attack
//        currentAttackPoint = Vector2.zero;
//        animator.SetBool("Is Attacking", false);
//    }
//}