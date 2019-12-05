using System.Collections.Generic;
using Ai;
using UnityEngine;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IGoap
    {
        public float speed;

        protected float terminalSpeed;
        protected float initialSpeed;
        protected float acceleration;
        protected float minDistance = 1.5f;
        protected float aggroDistance = 5.0f;
        protected bool loop = false;

        protected GameObject player;

        protected PolygonCollider2D enemy_Collider;
        protected Animator enemy_Animator;
        protected SpriteRenderer enemy_SpriteRenderer;
        protected Rigidbody2D enemy_RigidBody;

        public abstract HashSet<KeyValuePair<string, object>> createGoalState();
        public abstract void planFailed(HashSet<KeyValuePair<string, object>> failedGoal);
        public abstract void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);
        public abstract void actionsFinished();
        public abstract void planAborted(GoapAction aborter);

        public HashSet<KeyValuePair<string, object>> getWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
            worldData.Add(new KeyValuePair<string, object>("damagePlayer", false)); //to-do: change player's state for world data here
            worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
            return worldData;
        }

        public bool moveAgent(GoapAction nextAction)
        {
            float dist = Vector3.Distance(transform.position, nextAction.target.transform.position);
            if (dist < aggroDistance)
            {
                Vector3 moveDirection = player.transform.position - transform.position;

                SetSpeed(speed);

                if (initialSpeed < terminalSpeed)
                {
                    initialSpeed += acceleration;
                }

                Vector3 newPosition = moveDirection * initialSpeed * Time.deltaTime;
                transform.position += newPosition;
            }
            if (dist <= minDistance)
            {
                nextAction.setInRange(true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetSpeed(float val)
        {
            terminalSpeed = val / 10;
            initialSpeed = (val / 10) / 2;
            acceleration = (val / 10) / 4;
            return;
        } 
    }
}



