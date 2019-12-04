using System.Collections.Generic;
using UnityEngine;
using Ai.Fsm;

namespace Ai.Goap
{
    public sealed class GoapAgent : MonoBehaviour
    {
        private FSM stateMachine;

        private IFSMState idleState; // finds something to do
        private IFSMState moveToState; // moves to a target
        private IFSMState performActionState; // performs an action

        private HashSet<GoapAction> availableActions;
        private Queue<GoapAction> currentActions;

        /// <summary>
        /// this is the implementing class that provides our world data and listens to feed back on planning
        /// </summary>
        private IGoap dataProvider;

        //private GoapPlanner planner;
    }
}
