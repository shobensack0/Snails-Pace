using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float hitTimer = 5.0f;
    private float currenthitTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var renderer = this.GetComponent<SpriteRenderer>();
        if (collision.gameObject.tag == "PlayerHitCollision" && currenthitTimer == 0.0f)
        {
            renderer.color = new Color(0f, 0f, 0f, 1f); // Set to opaque black
            currenthitTimer = hitTimer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currenthitTimer > 0.0f)
        {
            currenthitTimer -= 0.5f * Time.deltaTime;
        }

        if (currenthitTimer == 0.0f)
        {



            var renderer = this.GetComponent<SpriteRenderer>();
            renderer.color = Color.white;
        }
    }
}
