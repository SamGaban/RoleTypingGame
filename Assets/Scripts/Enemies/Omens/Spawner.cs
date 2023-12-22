using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine;

public class Spawner : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private GameObject capsule;

    [TabGroup("references", "Data")] [ShowInInspector]
    private bool isGoingUp = true;

    [TabGroup("references", "Data")] [ShowInInspector]
    private GameObject enemy;

    [TabGroup("references", "Data")] [ShowInInspector]
    private GameObject currentPrefab;

    [TabGroup("references", "Data")] [ShowInInspector]
    private HealthManager healthManager;

    [TabGroup("references", "Data")] [ShowInInspector]
    private BaseEnemy currentlySpawnedEnemy;
    
    [TabGroup("settings", "Settings")] [SerializeField]
    private float MaxRandomSpawnTime = 10f;

    [TabGroup("settings", "Settings")] [SerializeField]
    private float MinRandomSpawnTime = 1f;
    
    private float birthTime = 8.35f;

    private float startTime = 0f;

    private bool hasActiveEnemy = false;

    private bool isDestroying = false;
    
    

    /// <summary>
    /// Starts the process of creating a new enemy
    /// </summary>
    public void SpawnEnemy(GameObject pref)
    {
        if (isDestroying) return;
        
        hasActiveEnemy = true;

        currentPrefab = pref;

        float RandomTime = Random.Random.Range(MinRandomSpawnTime, MaxRandomSpawnTime);

        Invoke("SpawnHelper", RandomTime);

    }

    /// <summary>
    /// Continuation of the spawn method, separated for invoking measures
    /// </summary>
    private void SpawnHelper()
    {
        enemy = Instantiate(currentPrefab);
        enemy.transform.SetParent(capsule.transform);

        // Reset local position, rotation, and scale
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
        enemy.transform.localScale = Vector3.one;

        // Your existing code
        capsule.transform.localPosition = new Vector3(0, -1f, 0);
        startTime = Time.time;
        isGoingUp = true;

        HealthManager script = enemy.GetComponent<HealthManager>();
        if (script != null)
        {
            healthManager = script;
        }

        BaseEnemy baseScript = enemy.GetComponent<BaseEnemy>();
        if (baseScript != null)
        {
            currentlySpawnedEnemy = baseScript;
        }
    }
    
    private void Update()
    {
        GoUp();
        StopGoingUp();
        DeathCheck();
    }

    /// <summary>
    /// Simple bool returning the state of the active enemy
    /// </summary>
    public bool HasActiveEnemy()
    {
        return hasActiveEnemy;
    }

    /// <summary>
    /// Makes the capsule go up from the grave
    /// </summary>
    private void GoUp()
    {
        if (!isGoingUp) return;
        
        capsule.transform.position += new Vector3(0f, 0.2f * Time.deltaTime, 0);
    }

    /// <summary>
    /// Stops the going up when the birthTime has been reached and activates the enemy
    /// </summary>
    private void StopGoingUp()
    {
        if (Time.time - startTime >= birthTime)
        {
            isGoingUp = false;
            
            if (enemy == null) return;
        
            BaseEnemy script = enemy.GetComponent<BaseEnemy>();
            if (script != null)
            {
                script.ActivateEnemy();
            }
        }
    }

    /// <summary>
    /// Checks if the assigned enemy is dead to then send the signal to the Omen that a new slot is free
    /// </summary>
    private void DeathCheck()
    {
        if (healthManager == null) return;
        
        if (!healthManager.isDead()) return;

        hasActiveEnemy = false;
        enemy = null;
        healthManager = null;
        currentlySpawnedEnemy = null;
    }

    #region DestroyMethods

    /// <summary>
    /// Kills associated enemy, blocks the spawning thread, self destructs
    /// </summary>
    public void SelfDestruct()
    {
        isDestroying = true;
        
        if (healthManager != null)
        {
            healthManager.Kill();
        }
        
        Invoke("DestroyHelper", 2.2f);
    }
    private void DestroyHelper()
    {
        Destroy(this.gameObject);
    }

    #endregion
    
}
