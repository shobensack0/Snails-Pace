using Ai;
using Player;
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
            TryGetComponent<Hulk>(out Hulk thisHulk);
            // TODO: abstract THESE COMPNONENTS BITCH
            target.TryGetComponent<Stats>(out Stats playerStats);
            target.TryGetComponent<BoxCollider2D>(out BoxCollider2D playerRigidBody);

            if (playerStats && playerRigidBody)
            {
                thisHulk.TryGetComponent<Rigidbody2D>(out Rigidbody2D thisHulk_RigidBody);

                if (thisHulk_RigidBody)
                {
                    var magnitude = 1000.0f;
                    var force = playerRigidBody.transform.position - transform.position;
                    force.Normalize();
                    playerStats.TakeDamage(thisHulk.strength, force * magnitude);
                }
            }

            return false;
        }
    }
}



