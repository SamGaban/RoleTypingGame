using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Common script to handle health of an entity, to combine with a slider (take from an entity that already has one)
/// </summary>
public class HealthManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _entityTransform;

    [SerializeField] Transform _canvasTransform;

    [SerializeField] Slider healthSlider;
    [Header("Settings")]
    [Range(1, 2000)]
    public int _healthPoints;
    private int _maxHealthPoints;

    private Vector3 defaultScale;
    private Vector3 reversedScale;

    private bool isDeceased = false;

    private void Start()
    {
        defaultScale = _canvasTransform.localScale;
        reversedScale = new Vector3(-_canvasTransform.localScale.x, _canvasTransform.localScale.y, _canvasTransform.localScale.z);
        _maxHealthPoints = _healthPoints;
    }

    public void SetHealth(int newHealth)
    {
        _healthPoints = newHealth;
    }
    
    private void Update()
    {
        if (isDeceased) return;

        if (_healthPoints <= 0)
        {
            isDeceased = true;
        }
        healthSlider.value = (float)_healthPoints / (float)_maxHealthPoints;
        KeepOrientation();
    }
    /// <summary>
    /// Works in relation with the entity's transform to keep orientation straight on rotation of the entity's sprite
    /// </summary>
    private void KeepOrientation()
    {
        if (Mathf.Sign(_entityTransform.localScale.x) > 0)
        {
            _canvasTransform.localScale = defaultScale;
        }
        else
        {
            _canvasTransform.localScale = reversedScale;
        }
    }
    /// <summary>
    /// Deals damage to the actual HP
    /// </summary>
    /// <param name="amount">amount of damage</param>
    public void DownHp(int amount)
    {
        if (_healthPoints > 0)
        {
            if (_healthPoints - amount >= 0)
            {
                _healthPoints -= amount;
            }
            else
            {
                _healthPoints = 0;
            }
        }
    }
    /// <summary>
    /// Heals an amount of HP to the actual HP
    /// </summary>
    /// <param name="amount">amount to heal</param>
    public void UpHp(int amount)
    {
        if (_healthPoints < _maxHealthPoints)
        {
            if (_healthPoints + amount <= _maxHealthPoints)
            {
                _healthPoints += amount;
            }
            else
            {
                _healthPoints = _maxHealthPoints;
            }
        }
    }
    /// <summary>
    /// Simple bool returning if the entity linked to this healthManager is dead
    /// </summary>
    /// <returns></returns>
    public bool isDead()
    {
        return isDeceased;
    }

}
