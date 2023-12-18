using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _gameObject;
    [SerializeField] Collider2D _collider;
    [SerializeField] Transform _transform;
    [SerializeField] Rigidbody2D _rb;
    [Header("Settings")]
    [SerializeField] float speed;
    private float _direction = 1;

    private Vector3 _normalScale;
    private Vector3 _reverserdScale;

    private Vector2 _moveInput;

    private void Start()
    {
        _moveInput = new Vector2(_direction * speed, 0);
        _normalScale = _transform.localScale;
        _reverserdScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
    }

    public void Inititalize(float direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        _rb.velocity = _moveInput;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);  // Destroy the fireball
        }

        Debug.Log("Collision Detected with: " + collision.gameObject.name);

        HealthManager _healthManager = null;
        _healthManager = collision.gameObject.GetComponent<HealthManager>();
        if (_healthManager != null)
        {
            _healthManager.DownHp(30);
        }
    }
}
