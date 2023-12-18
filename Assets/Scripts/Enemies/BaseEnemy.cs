using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _transform;
    [SerializeField] int dmgAmount;
    [SerializeField] SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;

    private bool hasDied = false;

    private Player _player;

    [SerializeField] HealthManager _healthManager;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        particleSystem = this.gameObject.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_healthManager.isDead()) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            _player.Hurt(dmgAmount);
        }
    }

    private void Update()
    {
        if (hasDied) return;
        
        if (_healthManager.isDead())
        {
            hasDied = true;
            // Invoke("Disappear", 2.5f);
            Invoke("OnParticleTrigger", 2.5f);
            // Invoke("DestroySelf", 4f);
        }
    }

    private void OnParticleTrigger()
    {
        particleSystem.Play();
        Debug.Log("Test");
    }

    private void Disappear()
    {
        spriteRenderer.gameObject.SetActive(false);
    }
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
