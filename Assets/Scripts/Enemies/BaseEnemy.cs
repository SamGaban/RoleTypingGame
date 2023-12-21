using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _transform;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private EnemyMove moveScript;
    [SerializeField] HealthManager _healthManager;
    private Player _player;
    [Header("Settings")]
    [SerializeField] int dmgAmount;

    [SerializeField] private float attackCoolDown = 5f;

    [SerializeField] private float knockBackForce;

    private float lastAttackTime;

    private bool hasDied = false;



    private void Start()
    {
        _player = FindObjectOfType<Player>();
        lastAttackTime = Time.time;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_healthManager.isDead()) return;

        if (!AttackCooledDown()) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockBack = new Vector2(knockBackForce * moveScript.Direction(), knockBackForce);
            
            _player.Hurt(dmgAmount, knockBack);
            lastAttackTime = Time.time;

            EnemyAnimation script = this.gameObject.GetComponent<EnemyAnimation>();
            if (script != null)
            {
                script.AttackAnim();
            }
        }
    }
    
    /// <summary>
    /// Bool to see if attack cooldown is done
    /// </summary>
    private bool AttackCooledDown()
    {
        return Time.time - lastAttackTime >= attackCoolDown;
    }
    
    private void Update()
    {
        if (hasDied) return;
        
        if (_healthManager.isDead())
        {
            hasDied = true;
            Invoke("DestroySelf", 2.5f);
        }
    }
    
    /// <summary>
    /// Self destruct
    /// </summary>
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
