using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isDebug = false;

    // config values
    public float defaultMoveSpeed = 5f;

    // game components
    public Rigidbody2D body;
    public Animator animator;
    public SpriteRenderer sprite;

    // effects
    public GameObject runDustCloud;

    // privates
    private Vector2 movement;
    private float directionX = 0.0f;
    private float directionY = -1.0f;
    private float moveSpeed = 5f;

    private float jumpTimer = 0.0f;

    private readonly float RUN_MULTIPLIER = 1.5f;
    private readonly float JUMP_TIME = 0.5f;

    // DEBUG
    private float directionDebugCastDistance = 1f;


    // Start is called before the first frame update
    void Start()
    {
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
        body.MovePosition(body.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void SetMovementInput()
    {
        // get inputs
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var isRunning = Input.GetAxisRaw("Fire3") > 0.0f;
        var jumpPressed = Input.GetAxisRaw("Jump") > 0.0f;

        // set motion states
        this.SetRunState(isRunning);
        this.SetJumpState(jumpPressed);

        // set direction in x and y
        this.SetDirection(horizontalMovement, verticalMovement, ref directionX, ref directionY);
        this.SetDirection(verticalMovement, horizontalMovement, ref directionY, ref directionX);

        // set movement
        movement = new Vector2(horizontalMovement, verticalMovement);
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
            else
            {
                otherDirection = 0;
            }
        }
    }

    private void SetRunState(bool isRunning)
    {
        // set running state
        if (isRunning)
        {
            moveSpeed = defaultMoveSpeed * RUN_MULTIPLIER;
            animator.speed = RUN_MULTIPLIER;
            ParticleSystemRenderer dustRenderer = null;

            // only emit if you're grounded and running
            if (animator.GetBool("Is Grounded"))
            {
                // create run effect
                if (directionY < 0)
                {
                    // we are facing down, so make change the order layer of the smoke
                    dustRenderer = runDustCloud.GetComponent<ParticleSystem>()?.GetComponent<ParticleSystemRenderer>();
                    dustRenderer.sortingOrder = 4;
                }
                else
                {
                    // otherwise back to 10
                    dustRenderer = runDustCloud.GetComponent<ParticleSystem>()?.GetComponent<ParticleSystemRenderer>();
                    dustRenderer.sortingOrder = 6;
                }

                Instantiate(runDustCloud, new Vector2(sprite.bounds.center.x, sprite.bounds.min.y), runDustCloud.transform.rotation);
            }
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
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

        Debug.DrawLine(transform.position, endRaycast, Color.magenta);
    }
}
