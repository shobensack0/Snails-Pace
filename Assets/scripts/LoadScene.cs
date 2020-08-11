using UnityEngine;

/// <summary>
/// Things that need to be loaded on every scene, should be loaded on the 
/// Scene Loader object on each scene
/// </summary>
public class LoadScene : MonoBehaviour
{
    void Start()
    {
        Dialoguer.Initialize();
    }
}
