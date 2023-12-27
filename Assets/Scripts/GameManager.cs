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

    public bool buildMode { get; private set; } = true;
    
    private int numberOfOmen = 0;

    public int PlayerGold { get; private set; } = 9999;

    [TabGroup("references", "data")] [ShowInInspector]
    public Dictionary<int, int> Buildables;


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
    
    public void ChangeDifficulty(Caster.DifficultyLevel newDiff)
    {
        difficultyLevel = newDiff;
    }

    private void Start()
    {
        Buildables = new Dictionary<int, int>();
        AddToBuildables(1);
        AddToBuildables(1);
        AddToBuildables(2);

    }
}
