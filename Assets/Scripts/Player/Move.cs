using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    private float originalMoveSpeed;
    
    [SerializeField] float jumpHeight = 5.0f;

    private float speedModifier = 1f;
    
    private bool allowExternalForces = false;

    Vector2 moveInput;
    float idleMoveTreshold = 1.5f;

    private bool startedMoving = false;
    private bool stoppedMoving = false;
    private bool jumping = false;
    
    // ############################# SPEED SPELL RELATED #########################

    public void SpeedUp(float modifier, float duration)
    {
        moveSpeed *= modifier;
        Invoke("ResetSpeedModifier", duration);
    }
    
    public void ResetSpeedModifier()
    {
        moveSpeed = originalMoveSpeed;
    }
    
    
    // ###########################################################################
    // ###########################################################################
    


    private void Start()
    {
        originalMoveSpeed = moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        if (_player.IsDead()) return;
        
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
        if (_player.IsDead()) return;
        
        if (PlayerIsCasting()) { return; }

        Jump();
    }
    
    private void FixedUpdate()
    {
        
        if (_player.IsDead()) return;
        
        if (allowExternalForces) return;

        SoundCheck();

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
    /// Method related to checks to player state for sound management
    /// </summary>
    public void SoundCheck()
    {
        if (_player.IsDead())
        {
            SoundMaster.Instance.PlayerFootstepsLoopEnd();
            return;
        }

        if (Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon)
        {
            if (!startedMoving)
            {
                stoppedMoving = false;
                startedMoving = true;
                SoundMaster.Instance.PlayerFootStepsLoopStart();
            }
            else if (!_rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                if (!jumping)
                {
                    jumping = true;
                    SoundMaster.Instance.PlayerFootstepsLoopEnd();
                }
            }
            else if (_rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                if (jumping)
                {
                    jumping = false;
                    SoundMaster.Instance.PlayerFootStepsLoopStart();
                }
            }
        }
        else if (Mathf.Abs(_rb.velocity.x) <= Mathf.Epsilon)
        {
            if (!stoppedMoving)
            {
                startedMoving = false;
                stoppedMoving = true;
                SoundMaster.Instance.PlayerFootstepsLoopEnd();
            }
        }
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
        _rb.velocity = new Vector2(moveSpeed * moveInput.x * speedModifier, _rb.velocity.y * speedModifier);
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
