using UnityEngine;

namespace Ai
{
    public interface FSMState
    {
        void Update(FSM fsm, GameObject gameObject);
    }
}
