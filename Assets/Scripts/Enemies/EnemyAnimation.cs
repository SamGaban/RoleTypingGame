using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ENEMY animator script
/// </summary>
public class EnemyAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] RuntimeAnimatorController _controller;
    [SerializeField] Animator _animator;
    [SerializeField] HealthManager _healthManager;

    private bool hasDied = false;

    private void Update()
    {
        if (hasDied) return;
        
        if (_healthManager.isDead())
        {
            hasDied = true;
            _animator.SetBool("isDead", true);
            return;
        }

        if (Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    /// <summary>
    /// Attack animation script
    /// <para>Turns other animations off for a while, while activating the attack one</para>
    /// </summary>
    public void AttackAnim()
    {
        _animator.SetBool("isRunning", false);
        
        _animator.SetBool("isAttacking", true);
        Invoke("TurnAttackOff", 0.4f);
    }
    /// <summary>
    /// Turns the animator bool isAttacking off
    /// </summary>
    private void TurnAttackOff()
    {
        _animator.SetBool("isAttacking", false);
    }
}
