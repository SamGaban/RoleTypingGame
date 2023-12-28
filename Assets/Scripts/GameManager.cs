using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton region

    

    
    // Static variable that holds the instance
    private static GameManager instance = null;

    // Public static means it can be accessed from anywhere
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    
    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // Assign it to the current object
            instance = this;

            // Make sure that it won't be destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
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

    public Dictionary<int, int> slotToSpellDic = new Dictionary<int, int>()
    {
        {1,1},
        {2,3},
        {3,4},
        {4,2}
    };

    
    // ################################# LAUNCHING A MISSION ###############################
    
    
    /// <summary>
    /// Parameter set on selection of a game contract, to set the number of omens in said game
    /// </summary>
    private int numberOfOmen = 0;

    private int goldReward = 0;

    public void SetOmenAmount(int amount)
    {
        numberOfOmen = amount;
    }

    public void SetGoldReward(int amount)
    {
        goldReward = amount;
    }
    
    
    // ######################################################################################
    
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

    public void AddGold(int amount)
    {
        PlayerGold += amount;
    }

    private void Start()
    {
        Buildables = new Dictionary<int, int>();
    
        AddGold(100);

    }
}
