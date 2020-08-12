using Player;
using UnityEngine;

namespace GameWorld
{
    /// <summary>
    /// Stuff you'll need everywhere.
    /// 
    /// Interactive game objects are things that the player can interact with via a button press (eg items, characters, etc).
    /// This is purposefully vauge to allow different objects to define their own interactivity.
    /// 
    /// This scripts children need to be attatched to a child of an object in order to work correctly.
    /// </summary>
    public class InteractiveMonoBehavior : MonoBehaviour
    {
        #region World Components
        protected GameObject player;
        protected Player.Player player_Script_Player;
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

            if (player != null)
                player.TryGetComponent<Player.Player>(out player_Script_Player);

            // these components belong to the parent object but can be used in scrips that e
            var parent = this.transform.parent;
            parent.TryGetComponent<PolygonCollider2D>(out character_Collider);
            parent.TryGetComponent<Animator>(out character_Animator);
            parent.TryGetComponent<SpriteRenderer>(out character_SpriteRenderer);
            parent.TryGetComponent<Rigidbody2D>(out character_RigidBody);

            TryGetComponent<CircleCollider2D>(out character_CircleCollider);
            TryGetComponent<SpriteRenderer>(out character_InteractivePromptSpriteRenderer);
        }
    }
}

