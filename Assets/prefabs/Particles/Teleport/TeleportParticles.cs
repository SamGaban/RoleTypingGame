using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script handling the teleport particles effect
/// </summary>
public class TeleportParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestruct", 6f);
    }

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
    
}
