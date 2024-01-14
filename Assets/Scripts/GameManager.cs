using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using UnityEngine;

//SINGLETON
/// <summary>
/// Class used for saving / Persistent data
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton region

    

    
    // Static variable that holds the instance
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private bool _alreadyLoaded = false;
    
    
    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // Assign it to the current object
            instance = this;

            // Make sure that it won't be destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);

            if (_alreadyLoaded) return;

            _alreadyLoaded = true;

            WholeLoad();

            musicVolume = ES3.Load<float>("musicVolume", 1.0f);

            effectsVolume = ES3.Load<float>("effectsVolume", 1.0f);

        }
        else if (instance != this)
        {
            // This enforces our singleton pattern, meaning there can only ever be one instance of this GameObject
            Destroy(gameObject);
        }

    }

    #endregion

    [SerializeField] private GameObject TestBuildable;
    
    public Caster.DifficultyLevel difficultyLevel = Caster.DifficultyLevel.Normal;



    public Dictionary<int, int> slotToSpellDic;

    public Dictionary<int, int> spellBuyState;

    public int omenCleansed;

    public long killCount;

    // ################################# SOUND RELATED  ###################################

    public float musicVolume;

    public float effectsVolume;

    // ################################# LAUNCHING A MISSION ###############################


    /// <summary>
    /// Parameter set on selection of a game contract, to set the number of omens in said game
    /// </summary>
    private int numberOfOmen = 12;

    private int goldReward = 0;

    /// <summary>
    /// Sets the amount of omen on clicking a questboard item
    /// </summary>
    /// <param name="amount">amount of omen in the next quest</param>
    public void SetOmenAmount(int amount)
    {
        numberOfOmen = amount;
    }
    /// <summary>
    /// Returns the amount of omen
    /// </summary>
    /// <returns>Amount of omen</returns>
    public int OmenAmount()
    {
        return numberOfOmen;
    }
    /// <summary>
    /// Sets gold reward on clicking a questboard item
    /// </summary>
    /// <param name="amount">Amount of gold for the quest</param>
    public void SetGoldReward(int amount)
    {
        goldReward = amount;
    }
    /// <summary>
    /// Returns gold reward for the current quest
    /// </summary>
    /// <returns>Current quest's gold reward</returns>
    public int GoldReward()
    {
        return goldReward;
    }
    
    
    // ######################################################################################
    
    /// <summary>
    /// Actual player gold (Loaded and saved)
    /// </summary>
    public int PlayerGold { get; private set; } = 0;

    /// <summary>
    /// Player's buildable inventory
    /// </summary>
    [TabGroup("references", "data")] [ShowInInspector]
    public Dictionary<int, int> Buildables;

    /// <summary>
    /// Adds a new entry or increments an existing one in the player's buildable inventory
    /// </summary>
    public void AddToBuildables(int buildable)
    {
        if (Buildables.ContainsKey(buildable))
        {
            Buildables[buildable] += 1;
        }
        else
        {
            Buildables.Add(buildable, 1);
        }
    }

    /// <summary>
    /// Removes a buildable from the player's inventory (stored in gamemanager)
    /// </summary>
    public void RemoveBuildable(int buildable)
    {
        if (Buildables[buildable] > 0)
        {
            Buildables[buildable]--;
        }

        if (Buildables[buildable] <= 0)
        {
            Buildables.Remove(buildable);
        }
    }
    
    /// <summary>
    /// Sets difficulty to the new parameter input difficulty (before launching a game)
    /// </summary>
    public void ChangeDifficulty(Caster.DifficultyLevel newDiff)
    {
        difficultyLevel = newDiff;
    }
    
    /// <summary>
    /// Subtracting gold on paying a buildable
    /// </summary>
    /// <param name="amount">Amount subtracted</param>
    /// <returns>true if enough gold to subtract, false otherwise</returns>
    public bool SubstractGold(int amount)
    {
        if (PlayerGold - amount >= 0)
        {
            PlayerGold -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Adds gold to the player's inventory after a quest
    /// </summary>
    /// <param name="amount">Amount of gold</param>
    public void AddGold(int amount)
    {
        PlayerGold += amount;
    }

    /// <summary>
    /// Load sequence (For things others than the town, which saves at every quest and return to town
    /// </summary>
    private void Start()
    {

    }



    /// <summary>
    /// Whole save sequences
    /// </summary>
    public void WholeSave()
    {
        try
        {
            SaveBuildables();
            SaveGold();
            SaveOmenCleansed();
            SaveKillCount();
            SaveSpellBuyState();
            SaveSlotDico();
            SaveTown();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save : {e.Message}");
        }


    }
    /// <summary>
    /// Whole load sequence
    /// </summary>
    public void WholeLoad() // WHEN ADDING DATA, DON'T FORGET TO ADD IT IN THE NEW GAME METHOD IN MAIN MENU FOR A CLEAN WIPE OF NEW GAME
    {
        try
        {
            LoadBuildables();
            LoadGold();
            LoadOmenCleansed();
            LoadKillCount();
            LoadSpellBuyState();
            LoadSlotDico();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load : {ex.Message}");
        }

    }
    
    //Individual parts of game saving / To put in whole save / load
    #region SAVE/LOAD

    public void SaveBuildables()
    {
        ES3.Save("savedBuildables", Buildables);
    }

    public void LoadBuildables()
    {
        Buildables = ES3.Load<Dictionary<int, int>>("savedBuildables", new Dictionary<int, int>());
    }

    public void SaveGold()
    {
        ES3.Save("savedGold", PlayerGold);
    }

    public void LoadGold()
    {
        PlayerGold = ES3.Load<int>("savedGold", 0);
    }

    public void SaveKillCount()
    {
        ES3.Save("savedKillCount", killCount);
    }

    public void LoadKillCount()
    {
        killCount = ES3.Load<long>("savedKillCount", 0L);
    }

    public void SaveSlotDico()
    {
        ES3.Save("slotDico", slotToSpellDic);
    }

    public void LoadSlotDico()
    {
        slotToSpellDic = ES3.Load<Dictionary<int, int>>("slotDico", new Dictionary<int, int>() { { 1, 1 }, {2, 3 } });
    }

    public void SaveSpellBuyState()
    {
        ES3.Save("SpellBuyState", spellBuyState);
    }

    private Dictionary<int, int> SpellValues = new Dictionary<int, int>()
    {
        {2, 15}, // FORCEFIELD
        {4, 25}, // GOOSPELL
        {5, 40} // TIME BEND
    };

    public void LoadSpellBuyState()
    {
        spellBuyState = ES3.Load<Dictionary<int, int>>("SpellBuyState", new Dictionary<int, int>() {{1, 0}, {3, 0}});

        foreach (KeyValuePair<int,int> entry in SpellValues)
        {
            if (!spellBuyState.ContainsKey(entry.Key))
            {
                spellBuyState.Add(entry.Key, entry.Value);
            }
        }
    }

    public void SaveOmenCleansed()
    {
        ES3.Save("omenCleansed", omenCleansed);
    }

    public void LoadOmenCleansed()
    {
        omenCleansed = ES3.Load<int>("omenCleansed", 0);
    }
    
    
    
    
    //ONLY WORKS IN BUILD MODE
    public void SaveTown()
    {

        int counter = 0;
        GameObject[] saveableObjects = GameObject.FindGameObjectsWithTag("Saveable");

        foreach (GameObject obj in saveableObjects)
        {
            // Create save data from the GameObject
            SaveableObjectData data = new SaveableObjectData(obj);


            // Save the data with a unique key
            string key = "Saveable_" + counter++;
            ES3.Save(key, data);
        }

        // Save the total count of saveable objects
        ES3.Save("Saveable_Count", counter);

    }

    //ONLY WORKS IN BUILD MODE
    public void LoadTown()
    {

        int totalSaveables = ES3.Load<int>("Saveable_Count");
        GameObject townParent = GameObject.FindGameObjectWithTag("TownParent");

        for (int counter = 0; counter < totalSaveables; counter++)
        {
            string key = "Saveable_" + counter;
            if (ES3.KeyExists(key))
            {

                // Load saved data
                SaveableObjectData data = ES3.Load<SaveableObjectData>(key);
                
                // Use the saved data to instantiate and set up the object
                GameObject prefab = BuildDico.Instance.dico[data.buildIndex];
                GameObject newInstance = Instantiate(prefab, data.position, data.rotation, townParent.transform);
                newInstance.transform.localScale = data.scale;
                SpriteRenderer sr = newInstance.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = data.sortOrder;
                }
                // Apply any other state or component data as necessary
            }
        }
 
    }


    [ButtonGroup]
    public void Motherlode()
    {
        PlayerGold += 1000;
    }


    
    #endregion
}
