using System.Collections.Generic;

namespace Ai.Goap
{
    public interface IGoap
    {
        /// <summary>
        /// The starting state of the Agent and the world. Supply what states are needed for actions to run.
        /// </summary>
        HashSet<KeyValuePair<string, object>> GetWorldState();

        /// <summary>
        /// Give the planner a new goal so it can figure out the actions needed to fulfill it.
        /// </summary>
        HashSet<KeyValuePair<string, object>> CreateGoalState();

        /// <summary>
        /// No sequence of actions could be found for the supplied goal. You will need to try another goal.
        /// </summary>
        void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

        /// <summary>
        /// A plan was found for the supplied goal. These are the actions the agent will perform, in order;
        /// </summary>
        void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);

        /// <summary>
        /// All actions are complete and the goal was reached. Hooray!
        /// </summary>
        void ActionsFinished();

        /// <summary>
        /// One of the actions caused the plan to abort. That action is returned.
        /// </summary>
        /// <param name="aborter"></param>
        void PlanAborted(GoapAction aborter);

        /// <summary>
        /// Called during Update. Move the agent towards the target in order for the next action to be able to perform.
        /// </summary>
        /// <returns>
        ///     True if the agent is at the target and the next action can perform.
        ///     False if it is not there yet.
        /// </returns>
        bool MoveAgent(GoapAction nextAction);
    }
}
