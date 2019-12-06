using Ai;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Hulk
{
    public class Hulk : Enemy
    {
        void Start()
        {
            minDist = 1.5f;
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
            character_Animator.SetTrigger("Kill");
        }

        public override void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal) { }
        public override void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> action) { }
        public override void ActionsFinished() { }
        public override void PlanAborted(GoapAction aborter) { }
    }
}



