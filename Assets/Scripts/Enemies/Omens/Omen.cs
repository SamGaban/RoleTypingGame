using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Enemy spawner motherclass
/// </summary>
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
    private Canvas canvas;

    [TabGroup("references", "References")] [SerializeField]
    private Slider slider;
    
    [TabGroup("references", "References")] [SerializeField]
    private Animator animator;
    
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> livesList;

    [TabGroup("references", "References")] [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [TabGroup("references", "Settings")] [SerializeField]
    private float timeToDestroyLife = 8f;

    [TabGroup("testing", "Data")] [ShowInInspector]
    private Dictionary<int, bool> hasItSpawnDictionary;

    [TabGroup("testing", "Data")] [ShowInInspector]
    private int livesCount = 3;

    [TabGroup("testing", "Data")] [ShowInInspector]
    public bool isDestroying = false;

    [TabGroup("testing", "Data")] [ShowInInspector]
    private bool hasTimerBegun = false;

    public bool hasPlayerOn = false;
    
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
        if (isDestroying) return;
        
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

    #region Lives Related

    private float startTime;
    
    /// <summary>
    /// Begins the countdown live down of an omen
    /// <para>If countdown already started, deduct 10 seconds
    /// </summary>
    [ButtonGroup("TestButtons")]
    public void LivesDown()
    {
        if (!hasTimerBegun)
        {
            startTime = Time.time;

            hasTimerBegun = true;
        }
        else
        {
            startTime -= 10f;
        }

    }

    private void LifeDownHelper()
    {
        SoundMaster.Instance.OmenLifeDown();
        Destroy(livesList[^1].gameObject);
        livesList.RemoveAt(livesList.Count -1);
        livesCount -= 1;
    }

    private void TimeCheck()
    {
        if (!hasTimerBegun) return;

        if (Time.time - startTime < timeToDestroyLife) return;

        hasTimerBegun = false;
        LifeDownHelper();
    }

    private void CanvasCheck()
    {
        if (!hasTimerBegun)
        {
            canvas.gameObject.SetActive(false);
            return;
        }

        canvas.gameObject.SetActive(true);
        slider.value = (timeToDestroyLife - (Time.time - startTime)) / timeToDestroyLife;
    }

    /// <summary>
    /// Checks if lives go to zero, if so, triggers the self destruct sequence, killing mobs / spawners / itself
    /// </summary>
    private void NoLivesCheck()
    {
        if (isDestroying) return;
        
        if (livesCount <= 0)
        {
            animator.SetTrigger("isGoingDown");
            
            isDestroying = true;
            
            Caster playerScript = FindObjectOfType<Caster>();

            if (playerScript != null)
            {
                playerScript.ForgetOmen();
            }
            
            foreach (GameObject o in spawnList)
            {
                Spawner script = o.GetComponent<Spawner>();
                script.SelfDestruct();
            }
            
            GameSession gameSession = FindObjectOfType<GameSession>();

            if (gameSession != null)
            {
                gameSession.KilledOmen();
            }
            
            
            Invoke("DestroyHelper", 8f);
        }
    }
    private void DestroyHelper()
    {

        
        Destroy(this.gameObject);
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
        NoLivesCheck();
        TimeCheck();
        CanvasCheck();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FeedOmenToPlayer(col);
            hasPlayerOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            MakePlayerForgetOmen(col);
            hasPlayerOn = false;
            hasTimerBegun = false;
        }
    }
}
