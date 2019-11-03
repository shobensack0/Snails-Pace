using UnityEngine;

public class PlayerPositionRender : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D player_BoxCollider;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.TryGetComponent<BoxCollider2D>(out player_BoxCollider);
        }

        TryGetComponent<SpriteRenderer>(out spriteRenderer);
        TryGetComponent<BoxCollider2D>(out collider);
    }

    void Update()
    {
        if (player_BoxCollider.transform.position.y <= collider.bounds.center.y)
        {
            // render object behind player
            this.spriteRenderer.sortingOrder = 0;
        } else
        {
            // render object in front of player
            this.spriteRenderer.sortingOrder = 10;
        }
    }
}
