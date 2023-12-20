using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Area of effect attack script
/// </summary>
public class Explosion : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5.0f; // Example radius
    private Collider2D[] hitColliders;

    /// <summary>
    /// Passes the damage of the explosion and makes it go boom
    /// </summary>
    /// <param name="damage"></param>
    public void Initialize(int damage)
    {
        hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, explosionRadius);

        if (hitColliders.Length <= 0) return;
        
        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Player"))
            {
                HealthManager healthManager = hitCollider.GetComponent<HealthManager>();
                if (healthManager != null)
                {
                    // Apply damage
                    healthManager.DownHp(damage); // explosionDamage is the damage amount
                }
            }
        } // Applying damage
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.attachedRigidbody != null)
            {
                // Calculate direction from explosion to the entity
                Vector3 direction = hitCollider.transform.position - this.gameObject.transform.position;
                direction.Normalize();

                // Apply force
                hitCollider.attachedRigidbody.AddForce(direction * 3f, ForceMode2D.Impulse); // forceAmount is the strength of the force
            }
        } // Applying forces
        
        Invoke("DestroySelf",1.5f);
        
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    
}
