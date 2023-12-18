using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDetection : MonoBehaviour
{
    private Rigidbody2D _playerRb;
    public Rigidbody2D _rb;
    [SerializeField] float jumpHeight;
    private bool readyToJump = true;

    public void FeedRb(Rigidbody2D playerRb)
    {
        _playerRb = playerRb;
    }


    private void Start()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerRb != null && readyToJump)
        {
            float verticalDistance = _playerRb.position.y - _rb.position.y;
            float horizontalDistance = Mathf.Abs(_playerRb.position.x - _rb.position.x);

            if (verticalDistance > 2f && _rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                readyToJump = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        readyToJump = true;
    }
}
