using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] RuntimeAnimatorController _controller;
    [SerializeField] Animator _animator;
    [SerializeField] HealthManager _healthManager;

    private bool hasDied = false;

    private void Update()
    {
        if (_healthManager.isDead()) return;

        if (Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDied) return;

        if (_healthManager.isDead())
        {
            hasDied = true;
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isAttacking", false);
            _animator.SetTrigger("isDead");
        }

        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("isAttacking", true);
            Invoke("TurnAttackOff", 1.2f);
        }
    }
    /// <summary>
    /// Turns the animator bool isAttacking off
    /// </summary>
    private void TurnAttackOff()
    {
        _animator.SetBool("isAttacking", false);
    }
}
