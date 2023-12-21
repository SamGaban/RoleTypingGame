using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [TabGroup("Settings")] [SerializeField]
    private int dmgAmount;

    [TabGroup("Settings")] [SerializeField]
    private float attackCoolDown = 5f;

    [TabGroup("Settings")] [SerializeField]
    private float knockBackForce;

    [Space] [SerializeField] [TabGroup("Settings")]
    private float moveSpeed;

    [Space] [SerializeField] [TabGroup("Settings")]
    private int healthPoints;

    [Space] [SerializeField] [TabGroup("Settings")]
    private float jumpHeight;

    [TabGroup("Animator Controller")] [SerializeField]
    private RuntimeAnimatorController Controller;
    
    [TabGroup("References")] [SerializeField]
    private Collider2D _collider;

    [TabGroup("References")] [SerializeField]
    private JumpDetection jumpScript;

    [TabGroup("References")] [SerializeField]
    private Rigidbody2D _rb;

    [TabGroup("References")] [SerializeField]
    private Transform _transform;

    [TabGroup("References")] [SerializeField]
    private SpriteRenderer spriteRenderer;

    [TabGroup("References")] [SerializeField]
    private EnemyMove moveScript;

    [TabGroup("References")] [SerializeField]
    private HealthManager _healthManager;
    private Player _player;

    [TabGroup("References")] [SerializeField]
    private EnemyAnimation animationScript;


    private float lastAttackTime;

    private bool hasDied = false;


    [ButtonGroup]
    private void Kill()
    {
        _healthManager.Kill();
    }
    
    [ButtonGroup]
    public void ActivateEnemy()
    {
        moveScript.ActivateEnemy();
    }
    

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        lastAttackTime = Time.time;
        
        _healthManager.SetHealth(healthPoints);
        
        jumpScript.SetJumpHeight(jumpHeight);
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

    public float MoveSpeed()
    {
        return moveSpeed;
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
