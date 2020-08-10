using GameWorld;
using UnityEngine;

namespace Player
{
    public class Interact : PlayerScriptMonoBehavior
    {
        #region Public Attributes
        private bool canInteract; // simply determines if we can interact currently 
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            this.SetCharacterComponents();
        }
    }

}
