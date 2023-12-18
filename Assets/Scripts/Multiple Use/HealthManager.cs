using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Transform _entityTransform;

    [SerializeField] Transform _canvasTransform;

    [SerializeField] Slider healthSlider;

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
    public bool isDead()
    {
        return isDeceased;
    }

}
