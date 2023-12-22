using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class Goospell : MonoBehaviour
{
    [TabGroup("settings", "settings")] [SerializeField]
    private float maxDuration = 35f;
    
    [TabGroup("settings", "settings")] [ShowInInspector]
    private float duration = 10f;

    [TabGroup("settings", "settings")] [ShowInInspector]
    private float slowAmount = 1f;
    
    [TabGroup("settings", "references")] [SerializeField]
    private Slider slider;


    private float startTime = 0;
    private bool hasStarted = false;
    

    private void Update()
    {
        if (!hasStarted) return;
        
        AutoDestroy();
        SliderUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyMove script = other.gameObject.GetComponent<EnemyMove>();
            if (script != null)
            {
                script.Slow(slowAmount);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyMove script = other.gameObject.GetComponent<EnemyMove>();
            if (script != null)
            {
                script.RestoreSpeed();
            }
        }
    }

    /// <summary>
    /// Starts the autodestroy sequence
    /// </summary>
    private void AutoDestroy()
    {
        if (Time.time - startTime < duration) return;

        Caster script = FindObjectOfType<Caster>();
        
        script.ForgetGoospell();
        
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Continuously updates the duration left slider
    /// </summary>
    private void SliderUpdate()
    {
        slider.value = (duration - (Time.time - startTime)) / duration;
    }

    /// <summary>
    /// Initialization script due when instantiated
    /// </summary>
    public void Init(int precision, int wordsPerMinute)
    {
        float truePrecision = precision / 100f;
        float trueWpm = wordsPerMinute / 60f;

        duration = maxDuration * truePrecision;
        slowAmount = trueWpm;
        startTime = Time.time;
        hasStarted = true;
    }
    
}
