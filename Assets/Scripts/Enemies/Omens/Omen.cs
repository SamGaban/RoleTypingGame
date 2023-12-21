using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Omen : MonoBehaviour
{
    #region References
    
    [TabGroup("references", "Mob List")] [SerializeField]
    private List<GameObject> prefabList;

    [TabGroup("references", "Spawns")] [SerializeField]
    private GameObject spawnsParent;

    [TabGroup("references", "Spawns")] [ShowInInspector]
    private List<GameObject> spawnList;
    
    [TabGroup("references", "References")] [SerializeField]
    private List<Sprite> mainOmenSpriteList;

    [TabGroup("references", "References")] [SerializeField]
    private SpriteRenderer spriteRenderer;

    [TabGroup("testing", "Data")] [ShowInInspector]
    private Dictionary<int, bool> hasItSpawnDictionary;


    #endregion
    
    
    #region InitMethods

    /// <summary>
    /// Returns a random sprite from the main sprites list
    /// </summary>
    private Sprite RandomMainSprite()
    {
        if (mainOmenSpriteList.Count == 1)
        {
            return mainOmenSpriteList[0];
        }
        else
        {
            return mainOmenSpriteList[Random.Range(0, mainOmenSpriteList.Count - 1)];
        }
    }

    /// <summary>
    /// Attributes a random sprite to the sprite renderer of the main Omen Object
    /// </summary>
    private void AttributeRandomSpriteMain()
    {
        spriteRenderer.sprite = RandomMainSprite();
    }

    /// <summary>
    /// Creates a state dictionary for each spawn point, with a bool indicating if spawn currently has a mob
    /// </summary>
    private void SpawnPointDictionaryInit()
    {
        hasItSpawnDictionary = new Dictionary<int, bool>();
        for (int i = 0; i < spawnList.Count; i++)
        {
            hasItSpawnDictionary.Add(i, false);
        }
    }

    /// <summary>
    /// Populates the spawnList by finding spawns in the children
    /// </summary>
    private void PopulateSpawnList()
    {
        spawnList = new List<GameObject>();
        
        foreach (Transform child in spawnsParent.transform)
        {
            if (child.CompareTag("OmenSpawn"))
            {
                spawnList.Add(child.gameObject);
            }
        }
    }
    
    #endregion


    #region Functionnalities

    // private void Spawn(GameObject enemy)

    /// <summary>
    /// Makes this omen the player's main omen target 
    /// </summary>
    private void FeedOmenToPlayer(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Caster script = other.gameObject.GetComponent<Caster>();

            if (script == null) return;
            
            script.FeedOmen(this.gameObject);
        }
    }

    /// <summary>
    /// Makes the player forget this omen as his main target
    /// </summary>
    private void MakePlayerForgetOmen(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Caster script = other.gameObject.GetComponent<Caster>();
            
            if (script == null) return;

            script.ForgetOmen();
        }
    }

    /// <summary>
    /// Sends signal to spawns that do not have a spawned enemy to begin the creation countdown (random)
    /// </summary>
    private void SpawnEnemy()
    {
        foreach (GameObject spawn in spawnList)
        {
            Spawner script = spawn.GetComponent<Spawner>();
            if (!script.HasActiveEnemy())
            {
                int randomEnemy = Random.Range(0, prefabList.Count);
                
                script.SpawnEnemy(prefabList[randomEnemy]);
            }
        }
    }
    
    #endregion
    
    private void Start()
    {
        PopulateSpawnList();
        AttributeRandomSpriteMain();
        SpawnPointDictionaryInit();

    }

    private void Update()
    {
        SpawnEnemy();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        FeedOmenToPlayer(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        MakePlayerForgetOmen(col);
    }
}
