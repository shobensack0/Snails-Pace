using System.Collections.Generic;
using Ai;
using UnityEngine;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IGoap
    {
        public int health;
        public int strength;
        public int speed;
        public Vector3 moveDirection;

        private float maxHitCooldown = 0.25f;
        private float hitCooldown = 1.0f;
        private float currentHitCooldown = 0.0f;

        protected float terminalSpeed;
        protected float initialSpeed;
        protected float acceleration;
        protected float minDist = 1.5f;
        protected float aggroDist = 5f;
        protected bool loop = false;

        protected GameObject player;

        protected PolygonCollider2D enemy_Collider;
        protected Animator enemy_Animator;
        protected SpriteRenderer enemy_SpriteRenderer;
        protected Rigidbody2D enemy_RigidBody;

        // Update is called once per frame
        public virtual void Update()
        {
            if (currentHitCooldown > 0.0f)
            {
                currentHitCooldown -= hitCooldown * Time.deltaTime;
            }
            else
            {
                enemy_Animator.speed = 1.0f;
                enemy_SpriteRenderer.color = Color.white;
            }
        }

        public HashSet<KeyValuePair<string, object>> getWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
            worldData.Add(new KeyValuePair<string, object>("damagePlayer", false)); //to-do: change player's state for world data here
            worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
            return worldData;
        }

        public abstract HashSet<KeyValuePair<string, object>> createGoalState();

        public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {

        }

        public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> action)
        {

        }

        public void actionsFinished()
        {

        }

        public void planAborted(GoapAction aborter)
        {

        }

        public void setSpeed(float val)
        {
            terminalSpeed = val / 10;
            initialSpeed = (val / 10) / 2;
            acceleration = (val / 10) / 4;
            return;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Weapon")
            {
                var magnitude = 500f;
                var force = transform.position - collision.transform.position;
                force.Normalize();
                this.enemy_RigidBody.AddForce(force * magnitude);

                enemy_Animator.speed = 0.25f;
                enemy_SpriteRenderer.color = Color.red;
                currentHitCooldown = maxHitCooldown;
            }
        }

        public virtual bool moveAgent(GoapAction nextAction)
        {
            var dist = Vector3.Distance(transform.position, nextAction.target.transform.position);

            if (dist < aggroDist)
            {
                moveDirection = player.transform.position - transform.position;
                setSpeed(speed);

                if (initialSpeed < terminalSpeed)
                {
                    initialSpeed += acceleration;
                }

                Vector3 newPosition = moveDirection * initialSpeed * Time.deltaTime;
                transform.position += newPosition;
            }

            DetermineMovementAndDirectionalState(moveDirection, initialSpeed);

            if (dist <= minDist)
            {
                nextAction.setInRange(true);
                return true;
            }
            else
            {
                initialSpeed = 0;
                return false;
            }

        }

        private void DetermineMovementAndDirectionalState(Vector3 direction, float speed)
        {
            if (enemy_Animator.GetFloat("Horizontal") != direction.x)
            {
                if (direction.x <= 0)
                    enemy_SpriteRenderer.flipX = true;
                else
                    enemy_SpriteRenderer.flipX = false;
            }

            // set animation values to their cooresponding directions
            enemy_Animator.SetFloat("Horizontal", direction.x);
            enemy_Animator.SetFloat("Vertical", direction.y);

            // sqrMagnitude will always be positive when we are moving
            enemy_Animator.SetFloat("Speed", speed);
        }
    }
}



