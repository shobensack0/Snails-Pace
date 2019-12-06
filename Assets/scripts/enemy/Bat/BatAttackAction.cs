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
            cost = 100f;
        }

        public override void reset()
        {
            attacked = false;
            target = null;
        }

        public override bool isDone()
        {
            return attacked;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            target = GameObject.Find("Player");
            return target != null;
        }

        public override bool perform(GameObject agent)
        {
            Bat currWolf = agent.GetComponent<Bat>();
            return false;
        }
    }
}



