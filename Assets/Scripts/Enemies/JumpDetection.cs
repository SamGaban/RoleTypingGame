using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to detect (using a collider) if there's a wall in front of the enemy
/// <para> if so, if the enemy's tracking the player, it will jump to get over the obstacle
/// </summary>
public class JumpDetection : MonoBehaviour
{
    private Rigidbody2D _playerRb;
    public Rigidbody2D _rb;
    [SerializeField] float jumpHeight;
    private bool readyToJump = true;
    [SerializeField] HealthManager _healthManager;

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
        if (_healthManager.isDead()) return;

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
        if (_healthManager.isDead()) return;

        readyToJump = true;
    }
}
