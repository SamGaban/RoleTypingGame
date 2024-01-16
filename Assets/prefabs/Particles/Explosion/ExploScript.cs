using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Script handling the explosion particle effect
/// </summary>
public class ExploScript : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private ParticleSystem particleSystemGoingRight;
    
    [TabGroup("references", "References")] [SerializeField]
    private ParticleSystem particleSystemGoingLeft;

    private float movingDirection = 0;

    /// <summary>
    /// Initializing the prefab'd explosion particle effect, giving it a direction, and then destroying it
    /// </summary>
    public void Init(float direction)
    {
        movingDirection = direction;
        
        if (direction == 1f)
        {
            particleSystemGoingRight.Play();
        }
        else
        {
            particleSystemGoingLeft.Play();
        }
        
        Invoke("SelfDestruct", 1.5f);
        
    }

    private void Update()
    {
        this.transform.position += new Vector3(0 + 1f * Time.deltaTime * movingDirection, 0, 0);
    }

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
    
}
