using System.Collections.Generic;
using Ai;
using UnityEngine;

namespace Enemy.Bat
{
    public class BatAttackAction : GoapAction
    {
        private bool attacked = false;

        public BatAttackAction()
        {
            addEffect("damagePlayer", true);
            cost = 100.0f;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            target = GameObject.Find("Player");
            return target != null;
        }

        public override bool isDone()
        {
            return attacked;
        }

        public override bool perform(GameObject agent)
        {
            var currBat = agent.GetComponent<BatV2>();

            // play attack animation
            return true;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override void reset()
        {
            attacked = false;
            target = null;
        }
    }
}



