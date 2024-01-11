using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private BaseEnemy baseScript;
    [SerializeField] Rigidbody2D _rb;
    [Header("Settings")]
    float speed;

    private float originalSpeed;
    
    [SerializeField] HealthManager _healthManager;

    private Rigidbody2D _playerRb;
    private Transform _transform;

    private Vector3 _baseOrientation;
    private Vector3 _reversedOrientation;

    private Vector2 _startPosition;

    private bool isPatrolling = true;
    private float changeDirectionCooldown = 0.1f;
    private float lastDirectionChange = 0f;

    public JumpDetection jumpScript;
    
    private bool allowExternalForces = false;


    Vector2 _moveInput;

    private float direction;

    private int patrolDirection;

    private bool deactivated = true;

    private bool notMoving = false;

    private bool stoppedMovingCheck = false;

    [TabGroup("references", "Data")]
    private float stoppedMovingTime;

    private int _currentWaypointIndex;

    [TabGroup("references", "Data")]
    private WaypointScript[] _waypoints;


    
    private void Start()
    {
        patrolDirection = Random.Range(0, 2);

        _waypoints = FindObjectsOfType<WaypointScript>()
         .Where(waypoint => Vector2.Distance(waypoint.transform.position, this.transform.position) <= 200)
         .OrderBy(waypoint => waypoint.index)
         .ToArray();

        int randomNumber = Random.Range(0, 2);

        _currentWaypointIndex = _waypoints[randomNumber == 0 ? 0 : _waypoints.Length - 1].index;

        
        _transform = this.gameObject.transform;
        _baseOrientation = _transform.localScale;
        _reversedOrientation = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.y);
        _transform.localScale = _baseOrientation;
        _startPosition = _rb.position;

        speed = baseScript.MoveSpeed();
        originalSpeed = baseScript.MoveSpeed();
    }

    public int CurrentWaypointIndex()
    {
        return _currentWaypointIndex;
    }

    public void JumpPadJump(float horizontalforce, float verticalforce, float reactivationtimeout)
    {
        allowExternalForces = true;
        _rb.AddForce(new Vector2(Mathf.Sign(this.transform.localScale.x) * horizontalforce, verticalforce), ForceMode2D.Impulse);
        Invoke("JumpPadJumpHelper", reactivationtimeout);
    }
    private void JumpPadJumpHelper()
    {
        allowExternalForces = false;
    }

    [ButtonGroup]
    /// <summary>
    /// Gives out the player location and stores it in the predeclared variable
    /// </summary>
    public void Detect()
    {
        if (_playerRb != null) return;
        
        isPatrolling = false;
        _playerRb = FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>();
        jumpScript.FeedRb(_playerRb);
    }
    public bool IsPatrolling()
    {
        return isPatrolling;
    }

    /// <summary>
    /// Checks if the enemy has been immobilized for more than 3 seconds and gives him a push if so, fixed the enemy getting stuck in stairs bug
    /// </summary>
    public void DebugPushCheck()
    {
        if (Mathf.Abs(_rb.velocity.x) > 1f)
        {
            notMoving = false;
            stoppedMovingCheck = true;
        }
        else if (stoppedMovingCheck)
        {
            stoppedMovingCheck = false;
            notMoving = true;
            stoppedMovingTime = Time.time;
        }

        if (notMoving && Time.time - stoppedMovingTime >= 3f)
        {
            DebugPush();
        }
    }
    
    /// <summary>
    /// When a player is detected, moves towards its location
    /// </summary>
    private void Move()
    {
        if (deactivated) return;

        if (_healthManager.isDead()) return;

        if (_playerRb != null)
        {
            _moveInput = new Vector2(Mathf.Sign(_playerRb.position.x - _rb.position.x) * speed, _rb.velocity.y);
        }
    }
    private void FixedUpdate()
    {
        if (_healthManager.isDead()) return;



        DebugPushCheck();



        // If the enemy is not patrolling, is activated, and is immobile, give him a little push in the direction he's facing
        if (Mathf.Abs(_rb.velocity.x) < Mathf.Epsilon && !deactivated && !isPatrolling) _rb.AddForce(new Vector2(1f * Mathf.Sign(_rb.velocity.x), 1f), ForceMode2D.Impulse);

        if (_playerRb != null)
        {
            Player script = _playerRb.gameObject.GetComponent<Player>();
            if (script != null)
            {
                if (script.IsDead()) deactivated = true;
            }
        }
        
        if (deactivated)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.velocity = Vector2.zero;
        }
        else
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
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (deactivated) return;

        if (collision.gameObject.CompareTag("Waypoint"))
        {
            WaypointScript script = collision.gameObject.GetComponent<WaypointScript>();

            if (script != null && script.index == _currentWaypointIndex && patrolDirection == 0)
            {
                //Debug.Log($"Reached waypoint {_currentWaypointIndex}, now going to waypoint {(_currentWaypointIndex + 1) % _waypoints.Length}");
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else if (script != null && script.index == _currentWaypointIndex && patrolDirection == 1)
            {
                _currentWaypointIndex = (_currentWaypointIndex - 1 + _waypoints.Length) % _waypoints.Length;
            }
        }
    }

    /// <summary>
    /// Returns the sign representing the direction the enemy is facingz
    /// </summary>
    /// <returns></returns>
    public float Direction()
    {
        return direction;
    }

    /// <summary>
    /// Forces the entity to turn the other way (debug)
    /// </summary>
    [ButtonGroup]
    public void TurnAround()
    {
        this.gameObject.transform.localScale = new Vector3(-this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
    }

    /// <summary>
    /// Sends a little force push on the entity after having been stuck for some seconds
    /// </summary>
    [ButtonGroup]
    public void DebugPush()
    {
        _rb.AddForce(new Vector2(20f * Mathf.Sign(this.transform.localPosition.x), 20f), ForceMode2D.Impulse); // HEEEEEEEEEEEEEEEEEEEEEEEEEEEERE
    }


    /// <summary>
    /// Makes the entity go from one patrol waypoint to the other
    /// </summary>
    private void Patrol()
    {
        if (deactivated) return;

        if (_healthManager.isDead()) return;


        _moveInput = new Vector2(Mathf.Sign(_waypoints[_currentWaypointIndex].transform.position.x - _rb.position.x) * speed, _rb.velocity.y);
        
    }

    /// <summary>
    /// Sends the entity back to patrol mode after having lost sight of the player for too long
    /// </summary>
    public void GoBackToPatrol()
    {
        isPatrolling = true;
        _playerRb = null;
    }

    
    /// <summary>
    /// Enables external forces to impact the RB of the enemy
    /// </summary>
    /// <param name="duration">duration</param>
    public void EnableExternalForces(float duration)
    {
        if (deactivated) return;

        allowExternalForces = true;
        Invoke("DisableExternalForces", duration);
    }

    [ButtonGroup]
    public void ActivateEnemy()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        deactivated = false;
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
        if (!(Time.time - lastDirectionChange > changeDirectionCooldown)) return;

        lastDirectionChange = Time.time;
        
        if (_rb.velocity.x > 0.2f)
        {
            _transform.localScale = _baseOrientation;

            direction = 1;
        }
        else if (_rb.velocity.x < -0.2f)
        {
            _transform.localScale = _reversedOrientation;
            direction = -1;
        }
    }

    #region Skill 4 related

    public void Slow(float efficiencyPercentage)
    {
        speed = speed / (4 * efficiencyPercentage);
    }

    public void RestoreSpeed()
    {
        speed = originalSpeed;
    }

    #endregion

}
