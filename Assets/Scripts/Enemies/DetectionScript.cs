using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionScript : MonoBehaviour
{

    [SerializeField] HealthManager _healthManager;

    [SerializeField] EnemyMove scriptMove;

    private float _lastSeen;

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

            _lastSeen = Time.time;
        }
    }

    /// <summary>
    /// updates the last seen time continually on trigger stay
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_healthManager.isDead()) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            _lastSeen = Time.time;
        }
    }

    /// <summary>
    /// Sends the entity back to patrol mode a given time after not seeing the player
    /// </summary>
    private void Update()
    {
        if ((Time.time - _lastSeen) >= 10f)
        {
            scriptMove.GoBackToPatrol();
        }
    }
}
