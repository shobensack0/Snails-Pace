using System.Collections.Generic;
using UnityEngine;

namespace Ai.Goap
{
    /// <summary>
    /// https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
    /// </summary>
    public abstract class GoapAction : MonoBehaviour
    {
        public HashSet<KeyValuePair<string, object>> Preconditions { get; set; }
        public HashSet<KeyValuePair<string, object>> Effects { get; set; }

        public bool inRange = false;

        /// <summary>
        /// The cost of performing the action. Figure out a weight that suits the action. 
        /// Changing it will affect what actions are chosen during planning
        /// </summary>
        public float cost = 1.0f;

        /// <summary>
        /// Actions often are performed on an object. This is that object. Can be null.
        /// </summary>
        public GameObject target;

        public GoapAction()
        {
            Preconditions = new HashSet<KeyValuePair<string, object>>();
            Effects = new HashSet<KeyValuePair<string, object>>();
        }

        public void DoReset()
        {
            inRange = false;
            target = null;
            Reset();
        }

        /// <summary>
        /// Reset any variables that need to be reset before planning happens again.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Determines if the action is done.
        /// </summary>
        public abstract void IsDone();

        /// <summary>
        /// Procedurally check if this action can run. Not all actions will need this.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns>Can action be run.</returns>
        public abstract bool CheckProceduralPrecondition(GameObject agent);

        /// <summary>
        /// Run the action
        /// </summary>
        /// <param name="agent"></param>
        /// <returns>
        ///     True if the action performed successfully.
        ///     False if something ahppened and it can no longer perform. In this case the action queue should
        /// celar out and the goal cannot be reached.
        /// </returns>
        public abstract bool Perform(GameObject agent);

        /// <summary>
        /// Determines if this action needs to be within range of a target game object. If not, then the moveTo
        /// state will not need to run for this action.
        /// </summary>
        /// <returns></returns>
        public abstract bool RequiresInRange();

        public void AddPrecondition(string key, object value)
        {
            Preconditions.Add(new KeyValuePair<string, object>(key, value));
        }

        public void RemovePrecondition(string key)
        {
            KeyValuePair<string, object> remove = default;

            foreach (KeyValuePair<string, object> kvp in Preconditions)
            {
                if (kvp.Key.Equals(key))
                    remove = kvp;
            }

            if (!default(KeyValuePair<string, object>).Equals(remove))
            {
                Preconditions.Remove(remove);
            }
        }

        public void AddEffect(string key, object value)
        {
            Effects.Add(new KeyValuePair<string, object>(key, value));
        }

        public void RemoveEffect(string key)
        {
            KeyValuePair<string, object> remove = default;

            foreach (KeyValuePair<string, object> kvp in Effects)
            {
                if (kvp.Key.Equals(key))
                    remove = kvp;
            }

            if (!default(KeyValuePair<string, object>).Equals(remove))
            {
                Effects.Remove(remove);
            }
        }
    }
}
