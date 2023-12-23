using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    
    public Caster.DifficultyLevel difficultyLevel = Caster.DifficultyLevel.Normal;

    public Dictionary<int, int> slotToSpellDic = new Dictionary<int, int>()
    {
        {1,1},
        {2,3},
        {3,4},
        {4,2}
    };


}
