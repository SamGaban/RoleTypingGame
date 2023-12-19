using System;
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
    [SerializeField] float ProjectionForce;
    [SerializeField] int Damage;
    private float _direction = 1;
    private int modifier;
    private int _wpmMod;

    private Vector3 _normalScale;
    private Vector3 _reverserdScale;

    private Vector2 _moveInput;

    private void Start()
    {
        _moveInput = new Vector2(_direction * speed, 0);
        _normalScale = _transform.localScale;
        _reverserdScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
    }

    public void Inititalize(float direction, int precisionMod, int wpmModif)
    {
        _direction = direction;
        modifier = precisionMod;
        _wpmMod = wpmModif;
    }

    private void Update()
    {
        _rb.velocity = _moveInput;
    }

    public void Launch(Rigidbody2D rigid)
    {
        Vector2 Projection = new Vector2(ProjectionForce * _direction, ProjectionForce);
        rigid.AddForce(Projection, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);  // Destroy the fireball
        }

        //If the collision has a health manager


        HealthManager _healthManager = null;
        _healthManager = collision.gameObject.GetComponent<HealthManager>();
        if (_healthManager != null)
        {
            EnemyMove enemyMoveScript = _healthManager.gameObject.GetComponent<EnemyMove>();

            if (enemyMoveScript != null)
            {
                enemyMoveScript.Detect();
            }
            
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            int resultDamage = DamageCalculator.CalculateDamage(Damage, modifier, _wpmMod);

            _healthManager.DownHp(resultDamage);

            Launch(rb);
            Destroy(gameObject);
        }
    }
}
