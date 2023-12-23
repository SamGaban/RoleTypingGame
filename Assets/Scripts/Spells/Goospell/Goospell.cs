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

    #region Raycast groundfinding region

    [TabGroup("settings", "settings")] [SerializeField]
    private float maxRaycastDistance = 100f;  // Maximum distance to check for the ground
    [TabGroup("settings", "settings")] [SerializeField]
    private LayerMask groundLayer;  // Assign the ground layer in the inspector

    void Start()
    {
        // Cast a ray downward from the spell's current position
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, Vector2.down, maxRaycastDistance, groundLayer);

        if (hit.collider != null)
        {
            // We hit something, move the spell to the ground level
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, hit.point.y, transform.position.z);
        }
    }
    
    
    #endregion
    

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
