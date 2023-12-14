using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    Vector2 moveInput;

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        
    }

    private void OnJump()
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        _rb.velocity = new Vector2(moveSpeed * moveInput.x, _rb.velocity.y);
        FlipSprite();
    }


    private void FlipSprite()
    {
        if (Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon)
        {
            _transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }
    }

}
