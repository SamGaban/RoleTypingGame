using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Hitter : MonoBehaviour
{

    [SerializeField] private Player playerScript;
    
    [SerializeField] private float attackReadyDuration = 1.0f; // Time in seconds the player can attack after activating

    private Coroutine attackReadyCoroutine;
    
    private bool canAttack = false;
    
    private bool enemyInRange = false;
    private Collider2D enemyCollider;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = true;
            enemyCollider = other;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = false;
            enemyCollider = null;
        }
    }
    private void Update()
    {
        if (playerScript.IsDead()) return;

        
        if (canAttack && enemyInRange)
        {
            canAttack = false;
            if (attackReadyCoroutine != null)
            {
                StopCoroutine(attackReadyCoroutine);
                attackReadyCoroutine = null;
            }
            AttackEnemy(enemyCollider);
        }
    }
    /// <summary>
    /// attacks enemy with spear
    /// </summary>
    /// <param name="enemy">enemy taken from the trigger</param>
    private void AttackEnemy(Collider2D enemy)
    {
        if (playerScript.IsDead()) return;
        
        HealthManager healthScript = enemy.GetComponent<HealthManager>();
        healthScript.DownHp(15);

        EnemyMove moveScript = enemy.gameObject.GetComponent<EnemyMove>();
        if (moveScript != null)
        {
            moveScript.EnableExternalForces(0.5f);
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            Vector2 knockBack = new Vector2(5f * playerScript.direction, 5f);
            rb.velocity += knockBack;
        }
        

    }
    /// <summary>
    /// Able to attack
    /// </summary>
    public void Activate()
    {
        canAttack = true;
        if (attackReadyCoroutine != null)
        {
            StopCoroutine(attackReadyCoroutine);
        }
        attackReadyCoroutine = StartCoroutine(ResetAttack());
    }
    /// <summary>
    /// Coroutine timer to reset the activated attack if nothing has been hit with the spear after a shot while
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackReadyDuration);
        canAttack = false;
    }
}
