using UnityEngine;

/// <summary>
/// Might not need this, just used from some tutorial for the running dust clouds
/// </summary>
public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}