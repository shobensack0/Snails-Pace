using UnityEngine;
using Player;

public class PowerFire : MonoBehaviour
{
    private Power script_Power = null;
    private Vector3 movementVector;
    private float speed = 0.0f;

    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (IsAlive())
        {
            this.transform.position += movementVector * Time.deltaTime;

            if (!this.GetComponent<SpriteRenderer>().isVisible)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnDestroy()
    {
    }

    public bool IsAlive()
    {
        return speed > 0.0f;
    }

    // main method when firing power
    public void Fire(Power power, Vector2 target, float speed)
    {
        this.script_Power = power;
        this.movementVector = ((Vector3) target - transform.position).normalized * speed;
        this.speed = speed;
    }
}
