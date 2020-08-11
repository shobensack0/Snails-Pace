using Player;
using UnityEngine;

namespace GameWorld
{
    /// <summary>
    /// Stuff you'll need everywhere.
    /// 
    /// NPC's are interactive, friendly characters in the game world.
    /// </summary>
    public abstract class InteractiveGameObject : InteractiveMonoBehavior
    {
        #region Attributes
        /// <summary>
        /// The radius of the circle collider that represents the area in which the player is able to interact with this object
        /// 
        /// This either gets set to a percent of the objects sprite by default, or can be set accordingly
        /// </summary>
        //public float interactRadius;
        #endregion

        #region Abstract Methods
        public abstract void SetColliderRadius();
        public abstract void HandlePlayerInProximity();
        #endregion

        public virtual void Start()
        {
            SetCharacterComponents();
            SetColliderRadius();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            //if (IsPlayerInProximity())
            //    HandlePlayerInProximity();
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                character_InteractivePromptSpriteRenderer.enabled = true;
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                character_InteractivePromptSpriteRenderer.enabled = false;
            }
        }
    }
}

