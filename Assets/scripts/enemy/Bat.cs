using UnityEngine;

namespace Enemy
{
    public class Bat : MonoBehaviour
    {
        #region Public stuff
        public float speed;
        #endregion 

        #region Player Stuff
        private GameObject player;
        #endregion

        #region Enemy Stuff
        private PolygonCollider2D enemy_Collider;
        private Animator enemy_Animator;
        private SpriteRenderer enemy_SpriteRenderer;

        private float maxHitCooldown = 0.25f;
        private float hitCooldown = 1.0f;
        private float currentHitCooldown = 0.0f;
        #endregion

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            TryGetComponent<PolygonCollider2D>(out enemy_Collider);
            TryGetComponent<Animator>(out enemy_Animator);
            TryGetComponent<SpriteRenderer>(out enemy_SpriteRenderer);
        }

        void Update()
        {
            if (currentHitCooldown > 0.0f)
            {
                currentHitCooldown -= hitCooldown * Time.deltaTime;
            }
            else
            {
                enemy_Animator.speed = 1.0f;
                enemy_SpriteRenderer.color = Color.white;
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed);
            }
        }


        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                enemy_Animator.speed = 0.25f;
                enemy_SpriteRenderer.color = Color.red;
                currentHitCooldown = maxHitCooldown;
            }
        }
    }
}



