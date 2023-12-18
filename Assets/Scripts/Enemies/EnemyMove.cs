using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [Header("Settings")]
    [SerializeField] float speed;
    [SerializeField] HealthManager _healthManager;

    private Rigidbody2D _playerRb;
    private Transform _transform;

    private Vector3 _baseOrientation;
    private Vector3 _reversedOrientation;

    public JumpDetection jumpScript;

    Vector2 _moveInput;

    private void Start()
    {
        _transform = this.gameObject.transform;
        _baseOrientation = _transform.localScale;
        _reversedOrientation = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.y);
        _transform.localScale = _baseOrientation;

    }

    public void Detect()
    {
        _playerRb = FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>();
        jumpScript.FeedRb(_playerRb);
    }

    private void Move()
    {
        if (_healthManager.isDead()) return;

        if (_playerRb != null)
        {
            _moveInput = new Vector2(Mathf.Sign(_playerRb.position.x - _rb.position.x) * speed, _rb.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (_healthManager.isDead()) return;

        Move();
        SpriteFlip();
        _rb.velocity = _moveInput;
    }

    private void SpriteFlip()
    {
        if (_rb.velocity.x > Mathf.Epsilon)
        {
            _transform.localScale = _baseOrientation;
        }
        else
        {
            _transform.localScale = _reversedOrientation;
        }
    }

}
