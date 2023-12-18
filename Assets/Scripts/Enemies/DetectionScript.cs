using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionScript : MonoBehaviour
{

    [SerializeField] HealthManager _healthManager;

    [SerializeField] EnemyMove scriptMove;

    /// <summary>
    /// Triggers the enemy detection script on entering
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_healthManager.isDead()) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            scriptMove.Detect();
        }
    }
}
