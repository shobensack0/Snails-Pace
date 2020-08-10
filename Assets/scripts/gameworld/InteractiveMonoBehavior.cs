using Player;
using UnityEngine;

namespace GameWorld
{
    /// <summary>
    /// Stuff you'll need everywhere.
    /// 
    /// Interactive game objects are things that the player can interact with via a button press (eg items, characters, etc).
    /// This is purposefully vauge to allow different objects to define their own interactivity
    /// </summary>
    public class InteractiveMonoBehavior : MonoBehaviour
    {
        #region World Components
        protected GameObject player;
        #endregion

        #region Character and Components
        protected PolygonCollider2D character_Collider;
        protected Animator character_Animator;
        protected SpriteRenderer character_SpriteRenderer;
        protected Rigidbody2D character_RigidBody;
        protected CircleCollider2D character_CircleCollider; // this is their "interactive circle"
        protected SpriteRenderer character_InteractivePromptSpriteRenderer; // this is the prompt that appears above the interacting object
        #endregion

        protected void SetCharacterComponents()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            TryGetComponent<PolygonCollider2D>(out character_Collider);
            TryGetComponent<Animator>(out character_Animator);
            TryGetComponent<SpriteRenderer>(out character_SpriteRenderer);
            TryGetComponent<Rigidbody2D>(out character_RigidBody);
            TryGetComponent<CircleCollider2D>(out character_CircleCollider);

            foreach(Transform child in this.transform)
            {
                if (child.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer interactivePRompteSpriteRenderer)) {
                    character_InteractivePromptSpriteRenderer = interactivePRompteSpriteRenderer;
                }
            }

            if (character_InteractivePromptSpriteRenderer == null)
                throw new System.Exception("yooo");
        }
    }
}

