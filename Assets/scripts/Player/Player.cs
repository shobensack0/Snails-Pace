using UnityEngine;

namespace Player
{
    public class Player : PlayerScriptMonoBehavior
    {
        #region Utilities
        public bool isDebug = false;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            this.SetCharacterComponents();
        }

        // Update is called once per frame
        void Update()
        {
            this.ConsoleDebug(isDebug);
        }

        private void ConsoleDebug(bool isDebug = false)
        { }
    }
}

#region neat code but not used
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

// htis code does that line draw thing
//var normalizer = (characterMovement.directionX != 0 && characterMovement.directionY != 0) ? 2 : 1;
//var endRayCastX = transform.position.x + ((directionDebugCastDistance * characterMovement.directionX) / normalizer);
//var endRayCastY = transform.position.y + ((directionDebugCastDistance * characterMovement.directionY) / normalizer);
//var endRaycast = new Vector2(endRayCastX, endRayCastY);

////Debug.DrawLine(transform.position, endRaycast, Color.magenta);
#endregion