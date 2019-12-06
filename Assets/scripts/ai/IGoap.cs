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
        HashSet<KeyValuePair<string, object>> getWorldState();

        HashSet<KeyValuePair<string, object>> createGoalState();

        void planFailed(HashSet<KeyValuePair<string, object>> failedGoal);

        void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);

        void actionsFinished();

        void planAborted(GoapAction aborter);

        bool moveAgent(GoapAction nextAction);
    }
}
