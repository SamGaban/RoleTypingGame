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
    [SerializeField] private GameObject explosionPrefab;
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
    /// <summary>
    /// Initializes this iteration of the prefab with needed values for direction / damage calculations
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="precisionMod"></param>
    /// <param name="wpmModif"></param>
    public void Inititalize(float direction, int precisionMod, int wpmModif)
    {
        _direction = direction;
        modifier = precisionMod;
        _wpmMod = wpmModif;
    }
    /// <summary>
    /// Moving
    /// </summary>
    private void Update()
    {
        _rb.velocity = _moveInput;
    }
    /// <summary>
    /// Entity move on impact
    /// </summary>
    /// <param name="rigid"></param>
    public void Launch(Rigidbody2D rigid)
    {
        Vector2 Projection = new Vector2(ProjectionForce * _direction, ProjectionForce);
        rigid.AddForce(Projection, ForceMode2D.Impulse);
    }
    /// <summary>
    /// Fireball impact
    /// </summary>
    /// <param name="collision">victim</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);  // Destroy the fireball
        }

        //If the collision has a health manager


        HealthManager _healthManager = collision.gameObject.GetComponent<HealthManager>();
        if (_healthManager != null)
        {
            EnemyMove enemyMoveScript = _healthManager.gameObject.GetComponent<EnemyMove>();

            if (enemyMoveScript != null)
            {
                enemyMoveScript.Detect();
                enemyMoveScript.EnableExternalForces(0.5f);
            }
            
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            int resultDamage = DamageCalculator.CalculateDamage(Damage, modifier, _wpmMod);

            // _healthManager.DownHp(resultDamage);
            
            // Instantiate explosion prefab
            
            GameObject explosion = Instantiate(explosionPrefab, _rb.position, Quaternion.identity);

            Explosion exploScript = explosion.GetComponent<Explosion>();
            
            exploScript.Initialize(resultDamage);

            Launch(rb);
            Destroy(gameObject);
        }
    }
}
