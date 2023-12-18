using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] RuntimeAnimatorController _controller;
    [SerializeField] Animator _animator;

    private void Update()
    {
        if (Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon)
        {
            _animator.SetBool("isRunning", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("isAttacking", true);
            Invoke("TurnAttackOff", 1.2f);
        }
    }

    private void TurnAttackOff()
    {
        _animator.SetBool("isAttacking", false);
    }
}
