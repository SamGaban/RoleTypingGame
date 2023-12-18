using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _transform;
    [SerializeField] int dmgAmount;

    private Player _player;

    [SerializeField] HealthManager _healthManager;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_healthManager.isDead()) return;

        if (collision.gameObject.tag == "Player")
        {
            _player.Hurt(dmgAmount);
        }
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
