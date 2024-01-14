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

    public bool deactivatedJump = false;

    private float verticalDistance; // Vertical distance with the player (not null if player is detected)
    private float horizontalDistance;

    private float lastJumpTime; // Last moment where a jump was performed per the entity


    /// <summary>
    /// This method is called every fixed frame-rate frame. It is used for applying physics-based updates.
    /// </summary>
    /// <remarks>
    /// This method is responsible for handling the logic of jumping and checking if the character is ready to jump.
    /// It also calculates the vertical and horizontal distance between the player and the character.
    /// </remarks>
    private void FixedUpdate()
    {

        if (_healthManager.isDead()) return;

        if (_rb.velocity.y > jumpHeight)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
        }

        if (_playerRb != null)
        {
            verticalDistance = _playerRb.position.y - _rb.position.y;
            horizontalDistance = Mathf.Abs(_playerRb.position.x - _rb.position.x);
        }

        if (!deactivatedJump && readyToJump && verticalDistance > 1f && horizontalDistance <= 2f && _rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            readyToJump = false;
            lastJumpTime = Time.time;
        }

        if (readyToJump) return;

        if (JumpCoolDown())

            if (_rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                readyToJump = true;
            }
    }


    /// <summary>
    /// Gets the actual jump height.
    /// </summary>
    /// <returns>The actual jump height.</returns>
    public float ActualJumpHeight()
    {
        return jumpHeight;
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

    /// <summary>
    /// Checks for collision of the jump check bar with a wall in front, and for tags also
    /// If the checks are good, makes the entity initiate its jump sequence
    /// </summary>
    /// <param name="collision">The Collider2D that this GameObject is colliding with.</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_healthManager.isDead()) return;

        if (!JumpCoolDown()) return;

        if (collision.gameObject.CompareTag("Player")
            || collision.gameObject.CompareTag("Forcefield")
            || collision.gameObject.CompareTag("Omen")
            || collision.gameObject.CompareTag("Spell")
            || collision.gameObject.CompareTag("Waypoint")
            || collision.gameObject.CompareTag("JumpPad")) return;

        if (_healthManager.isDead()) return;

        if (deactivatedJump) return;

        if (_ownMove.IsPatrolling() && readyToJump)
        {
            if (_rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                readyToJump = false;
                lastJumpTime = Time.time;
            }
        }

        if (_playerRb != null && readyToJump)
        {



            if ((verticalDistance > 2f && _rb.IsTouchingLayers(LayerMask.GetMask("Ground"))
                  || (_rb.velocity.x < 1 && horizontalDistance >= 4f)) && _rb.IsTouchingLayers(LayerMask.GetMask("Ground")))

            {
                _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                readyToJump = false;
                lastJumpTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Retrives the jump height from the global settings
    /// </summary>
    /// <param name="njumpHeight"></param>
    public void SetJumpHeight(float njumpHeight)
    {
        jumpHeight = njumpHeight;
    }
}
