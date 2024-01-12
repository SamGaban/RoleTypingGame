using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
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

    [TabGroup("Sounds")] [SerializeField]
    private AudioSource _deathSound;

    [TabGroup("Sounds")]
    [SerializeField]
    private AudioSource _walkSound;

    private float distance;
    
    private float lastAttackTime;

    private bool hasDied = false;


    /// <summary>
    /// This method is called by Unity during the scene view rendering loop.
    /// It is used to draw gizmos when the scene view is using deferred rendering.
    /// </summary>
    /// <remarks>
    /// This method sets the color of the Gizmos to green and then draws a wire sphere
    /// centered at the position of the transform with a radius of 10 units.
    /// </remarks>
    /// <seealso cref="Gizmos.color"/>
    /// <seealso cref="Gizmos.DrawWireSphere(Vector3, float)"/>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 10);
    }


    /// <summary>
    /// Kills the player or entity by calling the Kill method of the health manager.
    /// </summary>
    /// <remarks>
    /// This method is associated with an UI button group.
    /// </remarks>
    [ButtonGroup]
    private void Kill()
    {
        _healthManager.Kill();
    }
    
    /// <summary>
    /// Makes the entity able to move
    /// </summary>
    [ButtonGroup]
    public void ActivateEnemy()
    {
        moveScript.ActivateEnemy();
    }

    /// <summary>
    /// Activates the move script to feed the player's rb to the entity
    /// </summary>
    public void DetectPlayer()
    {
        moveScript.Detect();
    }
    

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        lastAttackTime = Time.time;
        
        _healthManager.SetHealth(healthPoints);
        
        jumpScript.SetJumpHeight(jumpHeight);
    }

    /// <summary>
    /// Handles the logic for the ongoing collision interaction between the enemy and other game entities.
    /// </summary>
    ///
    /// <param name="collision">The object associated with the ongoing collision.</param>
    /// <remarks>
    /// The method is triggered when there is an ongoing collision between the enemy entity and other game entities.
    /// If the collided object is tagged as 'Player':
    /// - It calculates the knockback force vector, which is a combination of the knockBackForce magnitude and the enemy's current direction for the 'x' component, and knockBackForce for the 'y' component.
    /// - It calls the 'Hurt' method on the 'Player' entity to apply damage and knockback from the ongoing collision.
    /// - It records the current time as the moment of the last attack of the enemy.
    /// - If a 'EnemyAnimation' component is attached to this enemy game object, it triggers the attack animation using the 'AttackAnim' method.
    /// </remarks>
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
    /// Retrieves the move speed value.
    /// </summary>
    /// <returns>The move speed value as a floating-point number.</returns>
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

    /// <summary>
    /// Updates the game object.
    /// </summary>
    /// <remarks>
    /// This method is responsible for updating the game object's state based on the current conditions.
    /// It calculates the distance between the player and the object's position,
    /// checks if the object has already died, and plays the death sound effect if the health manager indicates that the object is dead.
    /// After a delay of 2.5 seconds, it destroys itself.
    /// </remarks>
    private void Update()
    {
        distance = Vector2.Distance(_player.transform.position, this.transform.position);

        if (hasDied) return;


        if (_healthManager.isDead())
        {
            _deathSound.volume = GameManager.Instance.effectsVolume / (GameManager.Instance.effectsVolume + (distance / 10));
            _deathSound.Play();
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
