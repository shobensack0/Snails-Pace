using System.Collections.Generic;
using UnityEngine;

namespace Ai.Fsm
{
    /// <summary>
    /// Stack-based FSM.
    /// Push and pop states to FSM.
    /// 
    /// States should push other states onto the stack and pop themselves off.
    /// </summary>
    public class FSM
    {
        private Stack<IFSMState> stateStack = new Stack<IFSMState>();

        public delegate void IFSMState(FSM fsm, GameObject gameObject);

        public void Update(GameObject gameObject)
        {
            if (stateStack.Peek() != null)
            {
                stateStack.Peek().Invoke(this, gameObject);
            }
        }

        public void PushState(IFSMState state)
        {
            stateStack.Push(state);
        }

        public void PopState()
        {
            stateStack.Pop();
        }
    }
}
