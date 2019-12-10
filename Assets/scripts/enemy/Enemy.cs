using Ai;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Enemy
{
    public abstract class Enemy : EnemyMonoBehaviour, IGoap
    {

        #region Stats
        public int health;
        public int strength;
        public int speed;
        #endregion

        #region Movement Variables
        protected float terminalSpeed;
        protected float initialSpeed;
        protected float acceleration;

        protected Vector3 moveDirection;
        #endregion

        #region Hit Tracking
        protected float maxHitCooldown = 0.25f;
        protected float hitCooldown = 1.0f;
        protected float currentHitCooldown = 0.0f;
        protected bool takingDamage = false;

        protected bool isDead = false;
        #endregion

        #region AI
        protected float minDist;
        protected float aggroDist;
        protected bool loop = false;
        #endregion

        #region Abstract Methods
        public abstract void Kill();

        public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();
        public abstract void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);
        public abstract void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> action);
        public abstract void ActionsFinished();
        public abstract void PlanAborted(GoapAction aborter);
        #endregion

        // Update is called once per frame
        public virtual void Update()
        {
            DetermineHitStatusAndCooldown();
            CheckHealth();
        }

        public void CheckDeathState()
        {
            if (isDead && character_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(this.gameObject);
            }
        }

        public void CheckHealth()
        {
            CheckDeathState();

            Debug.Log(health + " ");
            if (health <= 0)
            {
                this.isDead = true;
                Kill();
            }
        }

        public HashSet<KeyValuePair<string, object>> GetWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
            worldData.Add(new KeyValuePair<string, object>("damagePlayer", false)); //to-do: change player's state for world data here
            worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
            return worldData;
        }

        public void SetSpeed(float val)
        {
            terminalSpeed = val / 10;
            initialSpeed = (val / 10) / 2;
            acceleration = (val / 10) / 4;
            return;
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Weapon")
            {
                var magnitude = 500f;
                var force = transform.position - collision.transform.position;
                force.Normalize();
                this.characte_RigidBody.AddForce(force * magnitude);

                character_Animator.speed = 0.25f;
                character_SpriteRenderer.color = Color.red;
                currentHitCooldown = maxHitCooldown;

                collision.gameObject.TryGetComponent<WeaponMonoBehavior>(out WeaponMonoBehavior weapon);

                this.health -= Mathf.RoundToInt(weapon.damageRange.GetDamageAmount());

                takingDamage = true;
            }
        }

        public virtual bool MoveAgent(GoapAction nextAction)
        {
            if (isDead)
                return false;

            var dist = Vector3.Distance(transform.position, nextAction.target.transform.position);

            if (dist < aggroDist)
            {
                moveDirection = player.transform.position - transform.position;
                SetSpeed(speed);

                if (initialSpeed < terminalSpeed)
                {
                    initialSpeed += acceleration;
                }

                Vector3 newPosition = moveDirection * initialSpeed * Time.deltaTime;
                transform.position += newPosition;
            }

            if (moveDirection != null)
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
            if (character_Animator.GetFloat("Horizontal") != direction.x)
            {
                if (direction.x <= 0)
                    character_SpriteRenderer.flipX = true;
                else
                    character_SpriteRenderer.flipX = false;
            }

            // set animation values to their cooresponding directions
            character_Animator.SetFloat("Horizontal", direction.x);
            character_Animator.SetFloat("Vertical", direction.y);

            // sqrMagnitude will always be positive when we are moving
            character_Animator.SetFloat("Speed", speed);
        }

        private void DetermineHitStatusAndCooldown()
        {
            if (takingDamage)
            {
                if (currentHitCooldown > 0.0f)
                {
                    currentHitCooldown -= hitCooldown * Time.deltaTime;
                }
                else
                {
                    takingDamage = false;
                    character_Animator.speed = 1.0f;
                    character_SpriteRenderer.color = Color.white;
                }
            }
        }
    }
}



