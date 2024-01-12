using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKillZoneScript : MonoBehaviour
{
    /// <summary>
    /// Kills an entity on impact with the kill zone
    /// </summary>
    /// <param name="collision">entity colliding with the kill zone</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthManager script = collision.gameObject.GetComponent<HealthManager>();

        if (script != null)
        {
            script.Kill();
        }
    }
}
