using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthManager script = collision.gameObject.GetComponent<HealthManager>();

        if (script != null)
        {
            script.Kill();
        }
    }
}
