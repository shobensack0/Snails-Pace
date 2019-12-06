using Ai;
using UnityEngine;

namespace Enemy.Hulk
{
    public class HulkAttackAction : GoapAction
    {
        private bool attacked = false;

        public HulkAttackAction()
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
            Hulk currWolf = agent.GetComponent<Hulk>();
            return false;
        }
    }
}



