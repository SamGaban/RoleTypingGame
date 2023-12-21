using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Script used to detect (using a collider) if there's a wall in front of the enemy
/// <para> if so, if the enemy's tracking the player, it will jump to get over the obstacle
/// </summary>
public class JumpDetection : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D _playerRb;
    public Rigidbody2D _rb;
    private bool readyToJump = true;
    [SerializeField] HealthManager _healthManager;
    [SerializeField] private EnemyMove _ownMove;
    [Header("Settings")]
    [SerializeField] float jumpHeight;
    public float maxVerticalVelocity = 5f;

    private float verticalDistance; // Vertical distance with the player (not null if player is detected)
    private float horizontalDistance;

    private float lastJumpTime; // Last moment where a jump was performed per the entity

    
    
    private void FixedUpdate()
    {
        if (_rb.velocity.y > jumpHeight)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
        }
        
        if (_playerRb != null)
        {
            verticalDistance = _playerRb.position.y - _rb.position.y;
            horizontalDistance = Mathf.Abs(_playerRb.position.x - _rb.position.x);
        }
        
        if (readyToJump && verticalDistance > 1f && horizontalDistance <= 2f && _rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            readyToJump = false;
        }

        if (readyToJump) return;
        
        if (JumpCoolDown())
        
        if (_rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            readyToJump = true;
            lastJumpTime = Time.time;
        }
    }
    /// <summary>
    /// Bool indicating if the jump has cooled down
    /// </summary>
    /// <returns></returns>
    private bool JumpCoolDown()
    {
        return Time.time - lastJumpTime >= 1.5f;
    }
    /// <summary>
    /// Publicly accessible script to feed the player's rb once found
    /// </summary>
    /// <param name="playerRb"></param>
    public void FeedRb(Rigidbody2D playerRb)
    {
        _playerRb = playerRb;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!JumpCoolDown()) return;
        
        if (collision.gameObject.CompareTag("Player")
            || collision.gameObject.CompareTag("Forcefield")
            || collision.gameObject.CompareTag("Omen")) return;
        
        if (_healthManager.isDead()) return;

        if (_ownMove.IsPatrolling() && readyToJump)
        {
            if (_rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                readyToJump = false;
            }
        }

        if (_playerRb != null && readyToJump)
        {

            

            if ((verticalDistance > 2f && _rb.IsTouchingLayers(LayerMask.GetMask("Ground")) 
                  || (_rb.velocity.x < 1 && horizontalDistance >= 4f)) && _rb.IsTouchingLayers(LayerMask.GetMask("Ground")))

            {
                _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                readyToJump = false;
            }
        }
    }

    public void SetJumpHeight(float njumpHeight)
    {
        jumpHeight = njumpHeight;
    }
}
