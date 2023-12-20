using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Script handling the force field skill use
/// <para> test
/// </summary>
public class Forcefield : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ownTransform;
    [SerializeField] private Collider2D ownCollider;
    
    
    private int _healthPoints = 1;

    private bool isDown = false;

    private bool isActive = false;

    private float minViableSize = 4.0f;
    
    private int _growAmount = 0;

    private float minHealth = 1f;

    private float maxHealth = 3000f;
    
    Vector3 minScale = new Vector3(4f, 4f, 4f);
    
    Vector3 maxScale = new Vector3(20f, 20f, 20f);
    
    
    
    
    
    
    private void FixedUpdate()
    {
        SizeCheck();
        DownCheck();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        Shrink(5);
    }

    /// <summary>
    /// Continuously checks on the healthPoints to grow/shrink shield in direct relation
    /// </summary>
    private void SizeCheck()
    {
        if (!isActive) return;
        
        // Map _healthPoints to a range of 0 to 1
        float lerpValue = (_healthPoints - minHealth) / (maxHealth - minHealth);

        // Interpolate between minScale and maxScale based on lerpValue
        ownTransform.localScale = Vector3.Lerp(minScale, maxScale, lerpValue);
        
    }
    /// <summary>
    /// Checks if health points reach 0, if so, turns the shield off and destroys it
    /// </summary>
    private void DownCheck()
    {
        if (_healthPoints <= 0)
        {
            isDown = true;
        }
        
        if (isDown)
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// Removes healthpoints
    /// </summary>
    /// <param name="amount"></param>
    public void Shrink(int amount)
    {
        _healthPoints -= amount;
    }
    /// <summary>
    /// Adds healthpoints
    /// </summary>
    /// <param name="amount"></param>
    public void Grow()
    {
        if (_healthPoints + _growAmount <= 3000)
        {
            _healthPoints += _growAmount;
        }
        else
        {
            _healthPoints = 3000;
        }
    }
    /// <summary>
    /// Returns actual healthpoints
    /// </summary>
    /// <returns>actual healthpoints</returns>
    public int ActualHealthPoints()
    {
        return _healthPoints;
    }
    /// <summary>
    /// Turns "isDown" to true, which destroys the shield
    /// </summary>
    public void KillShield()
    {
        isDown = true;
    }

    public bool IsActive()
    {
        return isActive;
    }
    
    public void ActivateShield(int totalChars)
    {
        isActive = true;
        ownCollider.enabled = true;
        ownTransform.localScale = new Vector3(minViableSize, minViableSize, minViableSize);
        _growAmount = Convert.ToInt32(3000 / totalChars);
    }
}
