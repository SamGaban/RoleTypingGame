using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameSession : MonoBehaviour
{

    
    
    [TabGroup("references", "Data")] [ShowInInspector]
    private int omenCount = 0;

    [TabGroup("references", "references")] [SerializeField]
    private HUD hud;

    [TabGroup("references", "references")] [SerializeField]
    private Transform playerTransform;

    [TabGroup("references", "references")] [SerializeField]
    private Canvas buildPanel;

    [TabGroup("references", "references")] [SerializeField]
    private CinemachineVirtualCamera mainCamera;
    
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

    [TabGroup("references", "data")] [ShowInInspector]
    private List<Omen> omenList = new List<Omen>();

    [TabGroup("references", "data")] [SerializeField]
    private GameObject objectToPlace;

    [TabGroup("references", "Data")] [ShowInInspector]
    private bool buildPanelOpen = false;
    

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
            omenList.Add(omen);
            omenCount++;
        }

        buildPanel.gameObject.SetActive(false);
    }


    
    public void ActivateEditMove(GameObject objectToEdit)
    {
        playerTransform.gameObject.SetActive(false);
        mainCamera.Follow = objectToEdit.transform;
        Cursor.visible = false;
    }

    public void DeactivateEditMove()
    {
        playerTransform.gameObject.SetActive(true);
        mainCamera.Follow = playerTransform;
        Cursor.visible = true;
    }

    public void OnBuild()
    {
        
        if (GameManager.Instance.buildMode)
        {
            if (buildPanelOpen)
            {
                buildPanel.gameObject.SetActive(false);
                buildPanelOpen = false;
            }
            else
            {
                buildPanel.gameObject.SetActive(true);
                buildPanelOpen = true;
            }
        }
    }
    
    
}
