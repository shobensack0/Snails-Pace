using Ai;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Hulk
{
    public class Hulk : Enemy
    {
        public GameObject killEffect;

        void Start()
        {
            minDist = 1.25f;
            aggroDist = 5f;

            health = 50;
            speed = 5;
            strength = 25;

            terminalSpeed = speed / 10;
            initialSpeed = (speed / 10) / 2;
            acceleration = (speed / 10) / 4;

            SetCharacterComponents();
        }

        public override HashSet<KeyValuePair<string, object>> CreateGoalState()
        {
            HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
            goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
            goal.Add(new KeyValuePair<string, object>("stayAlive", true));
            return goal;
        }

        public override void Kill()
        {
            ParticleSystemRenderer dustRenderer = killEffect.GetComponent<ParticleSystem>()?.GetComponent<ParticleSystemRenderer>();
            dustRenderer.sortingOrder = 6;

            Instantiate(killEffect, new Vector2(character_SpriteRenderer.bounds.center.x, character_SpriteRenderer.bounds.min.y), killEffect.transform.rotation);
        }

        public override void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal) { }
        public override void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> action) { }
        public override void ActionsFinished() { }
        public override void PlanAborted(GoapAction aborter) { }
    }
}



