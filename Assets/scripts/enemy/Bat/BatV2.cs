using System.Collections.Generic;
using Ai;
using UnityEngine;

namespace Enemy.Bat
{
    public class BatV2 : Enemy
    {
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            TryGetComponent<PolygonCollider2D>(out enemy_Collider);
            TryGetComponent<Animator>(out enemy_Animator);
            TryGetComponent<SpriteRenderer>(out enemy_SpriteRenderer);
            TryGetComponent<Rigidbody2D>(out enemy_RigidBody);

            speed = 20;

            terminalSpeed = speed / 10;
            initialSpeed = (speed / 10) / 2;
            acceleration = (speed / 10) / 4;
        }


        public override HashSet<KeyValuePair<string, object>> createGoalState()
        {
            HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
            goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
            goal.Add(new KeyValuePair<string, object>("stayAlive", true));
            return goal;
        }

        public override void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {
        }

        public override void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
        {
        }

        public override void actionsFinished()
        {
        }

        public override void planAborted(GoapAction aborter)
        {
        }
    }
}



