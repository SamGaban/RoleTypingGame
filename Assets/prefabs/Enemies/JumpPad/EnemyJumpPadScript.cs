using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpPadScript : MonoBehaviour
{
    [TabGroup("references", "Data")]
    [SerializeField]
    [Range(0,12)]
    private int _index;

    [TabGroup("references", "Data")]
    [SerializeField]
    [Range(1, 100)]
    private float HorizontalForce = 15f;

    [TabGroup("references", "Data")]
    [SerializeField]
    [Range(1, 100)]
    private float VerticalForce = 45f;

    [TabGroup("references", "Data")]
    [SerializeField]
    [Range(0, 12)]
    private float ReactivationTimeOut = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyMove script = collision.gameObject.GetComponent<EnemyMove>();

            if (script != null)
            {
                if (script.CurrentWaypointIndex() == _index)
                {
                    script.JumpPadJump(HorizontalForce, VerticalForce, ReactivationTimeOut);
                }
            }
        }
    }
}
