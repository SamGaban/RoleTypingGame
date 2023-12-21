using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PLAYER move script, takes input from input manager
/// </summary>
public class Move : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Player _player;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Collider2D _feetCollider;
    [SerializeField] Transform _transform;
    [Header("Settings")]
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpHeight = 5.0f;

    private bool allowExternalForces = false;

    Vector2 moveInput;
    float idleMoveTreshold = 1.5f;


    private void OnMove(InputValue value)
    {
        if (PlayerIsCasting()) { return; }

        moveInput = value.Get<Vector2>();
        MoveCheck();
    }

    /// <summary>
    /// Zeroes the actually stored moveInput
    /// </summary>
    public void StopMoveInput()
    {
        moveInput = Vector2.zero;
    }
    
    /// <summary>
    /// Input triggered jump
    /// </summary>
    private void OnJump()
    {
        if (PlayerIsCasting()) { return; }

        Jump();
    }
    
    private void FixedUpdate()
    {

        if (allowExternalForces) return;
        
        if (PlayerIsCasting())
        {
            _rb.velocity = new Vector2(0f, _rb.velocity.y);
            return;
        }

        FlipSprite();
        UpdateVelocity();
        JumpCheck();
        IdleCheck();
    }

    /// <summary>
    /// Allows external forces to apply on the player and deactivates his movement for a little time while
    /// <para> applying knockback
    /// </summary>
    /// <param name="force"></param>
    public void KnockBack(Vector2 force)
    {
        allowExternalForces = true;
        
        _rb.AddForce(force, ForceMode2D.Impulse);
        
        Invoke("TurnAllowExternalFalseAgain", 0.7f);
    }

    /// <summary>
    /// Trigger that disables the "Allow External forces" used for knockback
    /// </summary>
    public void TurnAllowExternalFalseAgain()
    {
        allowExternalForces = false;
    }
    
    /// <summary>
    /// Flips player sprite based rb's velocity / Direction
    /// </summary>
    private void FlipSprite()
    {
        if (moveInput.x > 0)
        {
            _transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            _transform.localScale = new Vector3(-1, 1, 1);
        }

    }
    /// <summary>
    /// Updates velocity based on the moveInput (from OnMove())
    /// </summary>
    private void UpdateVelocity()
    {
        _rb.velocity = new Vector2(moveSpeed * moveInput.x, _rb.velocity.y);
    }
    /// <summary>
    /// Adds impulse force to the rb's
    /// </summary>
    private void Jump()
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
    }
    /// <summary>
    /// Check to see if playerstate should be turned to running
    /// </summary>
    private void MoveCheck()
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _player.RunningState();
        }
    }
    /// <summary>
    /// Check to see if playerstate should be turned to jumping
    /// </summary>
    private void JumpCheck()
    {
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (_rb.velocity.y > Mathf.Epsilon)
            {
                _player.JumpingUpState();
            }
            else
            {
                _player.JumpingDownState();
            }
        }
        else
        {
            _player.RunningState();
        }
    }
    /// <summary>
    /// Check to see if the playerstate should be turned to Idling
    /// </summary>
    private void IdleCheck()
    {
        if (Mathf.Abs(_rb.velocity.x) < idleMoveTreshold && _feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _player.IdlingState();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Bool to check if player is casting right now</returns>
    private bool PlayerIsCasting()
    {
        return (_player.ActualState() == Player.state.Casting) ? true : false;
    }
    

}
