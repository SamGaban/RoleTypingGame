using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SpikeZoneScript : MonoBehaviour
{
     [TabGroup("settings", "settings")] [SerializeField]
    private float maxDuration = 35f;
    
    [TabGroup("settings", "settings")] [ShowInInspector]
    private float duration = 10f;

    [TabGroup("settings", "settings")] [ShowInInspector]
    private int damageAmount = 1;
    
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
            HealthManager script = other.gameObject.GetComponent<HealthManager>();

            if (script != null)
            {
                script.ToggleDot(true);
                script.updateDotDamage(damageAmount);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthManager script = other.gameObject.GetComponent<HealthManager>();

            if (script != null)
            {
                script.ToggleDot(false);
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

    switch(wordsPerMinute)
    {
        case <60:
            damageAmount = 3;
            break;
        case <100:
            damageAmount = 5;
            break;
        case <125:
            damageAmount = 7;
            break;
        case <150:
            damageAmount = 9;
            break;
        case <175:
            damageAmount = 11;
            break;
        case <200:
            damageAmount = 13;
            break;
        case <225:
            damageAmount = 15;
            break;
        case <250:
            damageAmount = 17;
            break;
        case <275:
            damageAmount = 19;
            break;
        case <300:
            damageAmount = 21;
            break;
        default:
            damageAmount = 23;
            break;
    }
    
        duration = maxDuration * truePrecision;
        startTime = Time.time;
        hasStarted = true;
    }
}
