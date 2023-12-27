using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Class serving as reference for all the existing prefabricated buildable items
/// </summary>
public class BuildDico : SerializedMonoBehaviour
{
    public static BuildDico Instance { get; private set; }

    [ShowInInspector]
    public Dictionary<int, GameObject> dico;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep it persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Optional: Ensure only one instance
        }
    }
}
    