using Ai;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Bat
{
    public class Bat : Enemy
    {
        void Start()
        {
            minDist = 1.5f;
            aggroDist = 5f;

            health = 50;
            speed = 20;
            strength = 5;

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
            Debug.Log("Bat ded");
            Destroy(this.gameObject);
        }

        public override void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal) { }
        public override void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> action) { }
        public override void ActionsFinished() { }
        public override void PlanAborted(GoapAction aborter) { }
    }
}



