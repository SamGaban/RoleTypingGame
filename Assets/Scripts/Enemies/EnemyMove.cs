using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [Header("Settings")]
    [SerializeField] float speed;
    [SerializeField] HealthManager _healthManager;

    private Rigidbody2D _playerRb;
    private Transform _transform;

    private Vector3 _baseOrientation;
    private Vector3 _reversedOrientation;

    public JumpDetection jumpScript;
    
    private bool allowExternalForces = false;

    Vector2 _moveInput;

    private void Start()
    {
        _transform = this.gameObject.transform;
        _baseOrientation = _transform.localScale;
        _reversedOrientation = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.y);
        _transform.localScale = _baseOrientation;

    }
    /// <summary>
    /// Gives out the player location and stores it in the predeclared variable
    /// </summary>
    public void Detect()
    {
        _playerRb = FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>();
        jumpScript.FeedRb(_playerRb);
    }
    /// <summary>
    /// When a player is detected, moves towards its location
    /// </summary>
    private void Move()
    {
        if (_healthManager.isDead()) return;

        if (_playerRb != null)
        {
            _moveInput = new Vector2(Mathf.Sign(_playerRb.position.x - _rb.position.x) * speed, _rb.velocity.y);
        }
    }
    private void FixedUpdate()
    {
        if (_playerRb == null)
        {
            _rb.velocity = Vector2.zero; // Need to put patrol here
        }
        
        if (_healthManager.isDead()) return;

        if (!allowExternalForces)
        {
            Move();
            SpriteFlip();
            _rb.velocity = _moveInput;
        }
    }
    /// <summary>
    /// Enables external forces to impact the RB of the enemy
    /// </summary>
    /// <param name="duration">duration</param>
    public void EnableExternalForces(float duration)
    {
        allowExternalForces = true;
        Invoke("DisableExternalForces", duration);
    }
    /// <summary>
    /// Disables external forces on the enemy's rb
    /// </summary>
    private void DisableExternalForces()
    {
        allowExternalForces = false;
    }
    /// <summary>
    /// Checks for enemy's direction to flip its sprite in the good direction
    /// </summary>
    private void SpriteFlip()
    {
        if (_rb.velocity.x > Mathf.Epsilon)
        {
            _transform.localScale = _baseOrientation;
        }
        else
        {
            _transform.localScale = _reversedOrientation;
        }
    }

}
