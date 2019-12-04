using UnityEngine;

namespace Ai.Fsm
{
    public interface IFSMState
    {
        void Update(FSM fsm, GameObject gameObject);
    }
}
