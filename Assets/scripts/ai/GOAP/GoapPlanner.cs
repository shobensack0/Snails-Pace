using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ai.Goap
{
    /// <summary>
    /// Plans what actions can be completed in order to fulfill a goal state
    /// </summary>
    public class GoapPlanner
    {
        /// <summary>
        /// Plan what sequence of actions can fullfill the goal.
        /// </summary>
        /// <returns>
        ///     Null if a plan could not be found
        ///     A list of the actions that must be performed, in order, to fulfill the goal
        /// </returns>
        public Queue<GoapAction> Plan(GameObject agent, 
            HashSet<GoapAction> availableActions,
            HashSet<KeyValuePair<string, object>> worldState,
            HashSet<KeyValuePair<string, object>> goal)
        {
            // reset the actions so we can start fresh with them
            foreach (GoapAction a in availableActions)
            {
                a.DoReset();
            }

            // check what actions can run using their CheckProceduralPreconditions
            var usableActions = new HashSet<GoapAction>();

            foreach(GoapAction action in availableActions)
            {
                if (action.CheckProceduralPrecondition(agent))
                {
                    usableActions.Add(action);
                }
            }

            // build up the tree and record the leaf nodes that provide a solution to the goal
            var leaves = new List<Node>();

            // build graph
            var start = new Node(null, 0, worldState, null);
            var success = BuildGraph(start, leaves, usableActions, goal);
        }

        /// <returns>
        ///     True if at least one solution was found.
        ///     
        ///     The possiblt paths are stored in the leaves list. Each leaf has a "running cost" value where the 
        ///     lowest cost will be the best action sequence.
        /// </returns>
        private bool BuildGraph(Node parent, List<Node> leaves, HashSet<GoapAction> usableActions, HashSet<KeyValuePair<string, object>> goal)
        {
            var foundOne = false;

            // go through each action available at this node and see if we can use it here
            foreach(var action in usableActions)
            {
                // if the parent state has the conditions for this actions preconditions, we can use it here
                if (InState(action.Preconditions, parent.state))
                {
                    var currentState = PopulateState(parent.state, action.Effects);
                }
            }
        }

        /// <summary>
        /// Check that all items in 'test' are in 'state'. If just one does not match or is not there then this returns false;
        /// </summary>
        private bool InState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
        {
            var allMatch = true;

            foreach (KeyValuePair<string, object> t in test)
            {
                var match = false;

                foreach (KeyValuePair<string, object> s in state)
                {
                    if (s.Equals(t))
                    {
                        match = true;
                        break;
                    }
                }

                if (!match)
                {
                    allMatch = false;
                }
            }
            return allMatch;
        }
    }
}
