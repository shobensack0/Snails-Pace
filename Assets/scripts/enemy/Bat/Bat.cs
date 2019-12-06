using System.Collections.Generic;
using Ai;
using UnityEngine;

namespace Enemy.Bat
{
    public class Bat : Enemy
    {
        void Start()
        {
            health = 50;
            speed = 20;
            strength = 10;

            terminalSpeed = speed / 10;
            initialSpeed = (speed / 10) / 2;
            acceleration = (speed / 10) / 4;


            player = GameObject.FindGameObjectWithTag("Player");

            TryGetComponent<PolygonCollider2D>(out enemy_Collider);
            TryGetComponent<Animator>(out enemy_Animator);
            TryGetComponent<SpriteRenderer>(out enemy_SpriteRenderer);
            TryGetComponent<Rigidbody2D>(out enemy_RigidBody);
        }

        public override HashSet<KeyValuePair<string, object>> createGoalState()
        {
            HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
            goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
            goal.Add(new KeyValuePair<string, object>("stayAlive", true));
            return goal;
        }
    }
}



