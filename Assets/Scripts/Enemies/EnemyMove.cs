using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    private Vector2 _startPosition;

    private bool isPatrolling = true;

    public JumpDetection jumpScript;
    
    private bool allowExternalForces = false;

    private bool goingRight = true;

    private bool goingLeft = false;

    Vector2 _moveInput;

    private void Start()
    {
        int randomNumber = Random.Range(0, 2);
        
        InvokeRepeating("RightLeftToggle", 1f, 5f);
        
        _transform = this.gameObject.transform;
        _baseOrientation = _transform.localScale;
        _reversedOrientation = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.y);
        _transform.localScale = _baseOrientation;
        _startPosition = _rb.position;
    }
    /// <summary>
    /// Gives out the player location and stores it in the predeclared variable
    /// </summary>
    public void Detect()
    {
        isPatrolling = false;
        _playerRb = FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>();
        jumpScript.FeedRb(_playerRb);
    }

    public bool IsPatrolling()
    {
        return isPatrolling;
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
            Patrol();
        }

        if (_healthManager.isDead())
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        if (!allowExternalForces)
        {
            Move();
            SpriteFlip();
            _rb.velocity = _moveInput;
        }
    }

    private void Patrol()
    {
        if (_healthManager.isDead()) return;

        _moveInput = new Vector2((goingRight ? 1 : -1) * speed, _rb.velocity.y);
    }

    private void RightLeftToggle()
    {
        goingRight = !goingRight;
        goingLeft = !goingLeft;
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
