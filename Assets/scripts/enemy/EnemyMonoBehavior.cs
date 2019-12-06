using UnityEngine;

namespace Enemy
{
    public abstract class EnemyMonoBehaviour : MonoBehaviour
    {
        #region World Components
        protected GameObject player;
        #endregion

        #region Character and Components
        protected PolygonCollider2D character_Collider;
        protected Animator character_Animator;
        protected SpriteRenderer character_SpriteRenderer;
        protected Rigidbody2D characte_RigidBody;
        #endregion

        protected void SetCharacterComponents()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            TryGetComponent<PolygonCollider2D>(out character_Collider);
            TryGetComponent<Animator>(out character_Animator);
            TryGetComponent<SpriteRenderer>(out character_SpriteRenderer);
            TryGetComponent<Rigidbody2D>(out characte_RigidBody);
        }
    }
}



