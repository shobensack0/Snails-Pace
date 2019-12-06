using System.Collections.Generic;

namespace Ai
{/**
 * Any agent that wants to use GOAP must implement
 * this interface. It provides information to the GOAP
 * planner so it can plan what actions to use.
 * 
 * It also provides an interface for the planner to give 
 * feedback to the Agent and report success/failure.
 */
    public interface IGoap
    {
        HashSet<KeyValuePair<string, object>> GetWorldState();

        HashSet<KeyValuePair<string, object>> CreateGoalState();

        void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

        void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);

        void ActionsFinished();

        void PlanAborted(GoapAction aborter);

        bool MoveAgent(GoapAction nextAction);
    }
}
