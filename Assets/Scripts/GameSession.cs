using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    
    
    [TabGroup("references", "Data")] [ShowInInspector]
    private int omenCount = 0;

    [TabGroup("references", "references")] [SerializeField]
    private HUD hud;
    
    public int OmenCount
    {
        get => omenCount;
        private set => omenCount = value;
    }
    
    [TabGroup("references", "Data")] [ShowInInspector]
    private int killCount = 0;

    public int KillCount
    {
        get => killCount;
        private set => killCount = value;
    }

    [TabGroup("references", "Data")] [ShowInInspector]
    private int omenKillCount = 0;

    public int OmenKillCount
    {
        get => omenKillCount;
        private set => omenKillCount = value;
    }
    

    public void KilledOmen()
    {
        omenCount--;
        omenKillCount++;
        hud.MinusOneOmen();

        if (omenCount <= 0)
        {
            Debug.Log("You Won!");
        }
        
    }


    
    void Start()
    {
        
        
        foreach (Omen omen in FindObjectsOfType<Omen>())
        {
            omenCount++;
        }
    }
}
